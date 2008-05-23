﻿#region MIT license
// 
// Copyright (c) 2007-2008 Jiri Moudry
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
#endregion

using System.Collections.Generic;
using System.Linq.Expressions;
using DbLinq.Linq.Data.Sugar.Expressions;

namespace DbLinq.Vendor
{
    public interface ISqlProvider
    {
        /// <summary>
        /// Converts a constant value to a literal representation
        /// </summary>
        /// <param name="literal"></param>
        /// <returns></returns>
        string GetLiteral(object literal);

        /// <summary>
        /// Converts a standard operator to an expression
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        string GetLiteral(ExpressionType operationType, IList<string> p);

        /// <summary>
        /// Converts a special expression type to literal
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        string GetLiteral(SpecialExpressionType operationType, IList<string> p);

        /// <summary>
        /// Places the expression into parenthesis
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        string GetParenthesis(string a);

        /// <summary>
        /// Returns a column related to a table.
        /// Ensures about the right case
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        string GetColumn(string table, string column);

        /// <summary>
        /// Returns a table alias
        /// Ensures about the right case
        /// </summary>
        /// <param name="table"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        string GetTableAsAlias(string table, string alias);

        /// <summary>
        /// Returns a table alias
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetTable(string table);

        /// <summary>
        /// Returns all table colums (*)
        /// </summary>
        /// <param name="tableAlias"></param>
        /// <returns></returns>
        string GetColumns(string tableAlias);

        /// <summary>
        /// Returns a literal parameter name
        /// </summary>
        /// <returns></returns>
        string GetParameterName(string nameBase);

        string GetFromClause(string[] tables);

        /// <summary>
        /// Joins a list of conditions to make a WHERE clause
        /// </summary>
        /// <param name="wheres"></param>
        /// <returns></returns>
        string GetWhereClause(string[] wheres);

        /// <summary>
        /// Returns a valid alias syntax for the given table
        /// </summary>
        /// <param name="nameBase"></param>
        /// <returns></returns>
        string GetTableAlias(string nameBase);
    }
}