#region MIT license
////////////////////////////////////////////////////////////////////
// MIT license:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//
// Authors:
//        Jiri George Moudry
////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DbLinq.Linq;
using DbLinq.Vendor;
using SqlMetal.Generator;
using SqlMetal.Generator.Implementation;
using SqlMetal.Util;

namespace SqlMetal.Generator.Implementation
{
    /// <summary>
    /// generates a c# class representing database.
    /// calls into CodeGenClass and CodeGenField.
    /// </summary>
    public class CSharpCodeGenerator: ICodeGenerator
    {
        const string NL = "\r\n";
        const string NLNL = "\r\n\r\n";

        public CSharpClassGenerator ClassGenerator { get; set; }
        public CSharpStoredProcedureGenerator StoredProcedureGenerator { get; set; }

        public CSharpCodeGenerator()
        {
            ClassGenerator = new CSharpClassGenerator();
            StoredProcedureGenerator = new CSharpStoredProcedureGenerator();
        }

        public string Extension
        {
            get { return ".cs"; }
        }

        public void Write(TextWriter textWriter, DlinqSchema.Database dbSchema, SqlMetalParameters parameters, string dataContextBaseType)
        {
            using (var codeWriter = new CodeWriter(textWriter))
            {
                string code = GetAll(dbSchema, parameters, dataContextBaseType);
                codeWriter.Write(code);
            }
        }

        public string GetAll(DlinqSchema.Database dbSchema, SqlMetalParameters parameters, string dataContextBaseType)
        {
            if (dbSchema == null || dbSchema.Tables == null)
            {
                Console.WriteLine("CodeGenAll ERROR: incomplete dbSchema, cannot start generating code");
                return null;
            }

            string prolog = @"
//#########################################################################
// Generated by DbLinq SqlMetal on $date - extracted from $db.
//#########################################################################

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.Linq.Mapping;
using System.Reflection;
using DbLinq.Linq;
using DbLinq.Linq.Mapping;
";
            //prolog = prolog.Replace("$dataContext", vendor.DataContextName());

            List<string> classBodies = new List<string>();


            foreach (DlinqSchema.Table tbl in dbSchema.Tables)
            {
                string classBody = ClassGenerator.GetClass(dbSchema, tbl, parameters);
                classBodies.Add(classBody);
            }

            string opt_namespace = @"
namespace $ns
{
    $classes
}
";
            string prolog1 = prolog.Replace("$date", DateTime.Now.ToString("yyyy-MMM-dd"));
            string source = parameters.Server != null ? "server " + parameters.Server : "file " + parameters.SchemaXmlFile;
            //prolog1 = prolog1.Replace("$db", parameters.Server);
            prolog1 = prolog1.Replace("$db", source);
            string classesConcat = string.Join(NLNL, classBodies.ToArray());
            classesConcat = GenerateDbClass(dbSchema, dataContextBaseType) + NLNL + classesConcat;
            string fileBody;
            if (parameters.Namespace == null || parameters.Namespace == "")
            {
                fileBody = prolog1 + classesConcat;
            }
            else
            {
                string body1 = opt_namespace;
                body1 = body1.Replace("$ns", parameters.Namespace);
                classesConcat = classesConcat.Replace(NL, NL + "\t"); //move text one tab to the right
                body1 = body1.Replace("$classes", classesConcat);
                fileBody = prolog1 + body1;
            }
            return fileBody;
        }

        private string GenerateDbClass(DlinqSchema.Database dbSchema, string dataContextBaseType)
        {
            //if (tables.Count==0)
            //    return "//L69 no tables found";
            if (dbSchema.Tables.Count == 0)
                return "//L69 no tables found";

            const string dbClassStr = @"
/// <summary>
/// This class represents $vendor database $dbname.
/// </summary>
public partial class $dbname : $dataContext
{
//    public $dbname(string connectionString) 
//        : base(connectionString)
//    {
//    }
    public $dbname(IDbConnection connection) 
        : base(connection)
    {
    }

    //these fields represent tables in database and are
    //ordered - parent tables first, child tables next. Do not change the order.
    $fieldDecl

    $storedProcs
}";
            // picrap: why must we not change the order?
            string dbName = dbSchema.Class;
            if (dbName == null)
            {
                dbName = dbSchema.Name;
            }

            List<string> dbFieldDecls = new List<string>();
            List<string> dbFieldInits = new List<string>();
            foreach (DlinqSchema.Table tbl in dbSchema.Tables)
            {
                //string fldDecl = string.Format("public Table<{1}> {0} {{ get {{ return base.GetTable<{1}>(\"{2}\"); }} }}",
                //                               tbl.Member, tbl.Type.Name, tbl.Name);
                string fldDecl = string.Format("public Table<{1}> {0} {{ get {{ return base.GetTable<{1}>(); }} }}",
                                               tbl.Member, tbl.Type.Name);
                dbFieldDecls.Add(fldDecl);

                string fldInit = string.Format("{0} = new Table<{1}>(this);",
                                               tbl.Member, tbl.Type.Name);
                dbFieldInits.Add(fldInit);
            }

            //obsolete - we cannot declare a field for each table - it's a problem for large DBs
            //string dbFieldInitStr = string.Join(NL + "\t\t", dbFieldInits.ToArray());
            string dbFieldDeclStr = string.Join(NL + "\t", dbFieldDecls.ToArray());

            List<string> storedProcList = new List<string>();
            foreach (DlinqSchema.Function storedProcedure in dbSchema.Functions)
            {
                string s = StoredProcedureGenerator.GetProcedureCall(storedProcedure);
                storedProcList.Add(s);
            }
            
            if (storedProcList.Count > 0)
            {
                storedProcList.Insert(0, "#region stored procs");
                storedProcList.Add("#endregion");
            }

            string storedProcsStr = string.Join(NLNL, storedProcList.ToArray());

            string dbs = dbClassStr;
            dbs = dbs.Replace("    ", "\t"); //for spaces mean a tab
            dbs = dbs.Replace("$dataContext", dataContextBaseType);
            dbs = dbs.Replace("$dbname", dbName);
            //dbs = dbs.Replace("$fieldInit", dbFieldInitStr); //no more tables as fields
            dbs = dbs.Replace("$fieldDecl", dbFieldDeclStr);
            dbs = dbs.Replace("$storedProcs", storedProcsStr);
            return dbs;
        }

    }
}