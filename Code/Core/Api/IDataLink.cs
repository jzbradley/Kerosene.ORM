﻿using Kerosene.Tools;
using System;

namespace Kerosene.ORM.Core
{
	// ==================================================== 
	/// <summary>
	/// Represents an agnostic connection with a database-alike service.
	/// </summary>
	public interface IDataLink : IDisposableEx, ICloneable
	{
		/// <summary>
		/// Returns a new instance that is a copy of the original one.
		/// </summary>
		/// <returns>A new instance.</returns>
		new IDataLink Clone();

		/// <summary>
		/// The engine this link is associated with.
		/// </summary>
		IDataEngine Engine { get; }

		/// <summary>
		/// The nestable transaction this instance maintains, created on-demand if needed (for
		/// instance if the previous reference is disposed).
		/// </summary>
		INestableTransaction Transaction { get; }

		/// <summary>
		/// Gets or sets the default transaction mode to use when creating a new transaction
		/// for this instance.
		/// <para>The setter may also fail if the mode is not supported by the concrete instance.</para>
		/// </summary>
		NestableTransactionMode DefaultTransactionMode { get; set; }

		/// <summary>
		/// Opens the connection against the database-alike service.
		/// <para>The framework invokes this method automatically when needed.</para>
		/// <para>Invoking this method in an opened link may throw an exception.</para>
		/// </summary>
		void Open();

		/// <summary>
		/// Closes the connection that might be opened against the database-alike service.
		/// <para>The framework invokes this method automatically when needed.</para>
		/// </summary>
		void Close();

		/// <summary>
		/// Whether this connection can be considered opened or not.
		/// </summary>
		bool IsOpen { get; }

		/// <summary>
		/// Creates a new raw command for this link.
		/// </summary>
		/// <returns>The new command.</returns>
		IRawCommand Raw();

		/// <summary>
		/// Creates a new raw command for this link and sets its initial contents using the text
		/// and arguments given.
		/// </summary>
		/// <param name="text">The new text of the command. Embedded arguments are specified
		/// using the standard '{n}' positional format.</param>
		/// <param name="args">An optional collection containing the arguments specified in the
		/// text set into this command.</param>
		/// <returns>The new command.</returns>
		IRawCommand Raw(string text, params object[] args);

		/// <summary>
		/// Creates a new raw command for this link and sets its initial contents by parsing the
		/// dynamic lambda expression given.
		/// </summary>
		/// <param name="spec">A dynamic lambda expression that resolves into the logic of this
		/// command. Embedded arguments are extracted and captured automatically in order to
		/// avoid injection attacks.</param>
		/// <returns>The new command.</returns>
		IRawCommand Raw(Func<dynamic, object> spec);

		/// <summary>
		/// Creates a new query command for this link.
		/// </summary>
		/// <returns>The new command.</returns>
		IQueryCommand Query();

		/// <summary>
		/// Creates a new query command for this link and sets the contents of its FROM clause.
		/// </summary>
		/// <param name="froms">The collection of lambda expressions that resolve into the
		/// elements to include in this clause:
		/// <para>- A string, as in 'x => "name AS alias"', where the alias part is optional.</para>
		/// <para>- A table specification, as in 'x => x.Table.As(alias)', where both the alias part
		/// is optional.</para>
		/// <para>- Any expression that can be parsed into a valid SQL sentence for this clause.</para>
		/// </param>
		/// <returns>The new command.</returns>
		IQueryCommand From(params Func<dynamic, object>[] froms);

		/// <summary>
		/// Creates a new query command for this link and sets the contents of its SELECT clause.
		/// </summary>
		/// <param name="selects">The collection of lambda expressions that resolve into the
		/// elements to include into this clause:
		/// <para>- A string, as in 'x => "name AS alias"', where the alias part is optional.</para>
		/// <para>- A table and column specification, as in 'x => x.Table.Column.As(alias)', where
		/// both the table and alias parts are optional.</para>
		/// <para>- A specification for all columns of a table using the 'x => x.Table.All()' syntax.</para>
		/// <para>- Any expression that can be parsed into a valid SQL sentence for this clause.</para>
		/// </param>
		/// <returns>The new command.</returns>
		IQueryCommand Select(params Func<dynamic, object>[] selects);

		/// <summary>
		/// Creates a new insert command for this link.
		/// </summary>
		/// <param name="table">A dynamic lambda expression that resolves into the table the new
		/// command will refer to.</param>
		/// <returns>The new command.</returns>
		IInsertCommand Insert(Func<dynamic, object> table);

		/// <summary>
		/// Creates a new delete command for this link.
		/// </summary>
		/// <param name="table">A dynamic lambda expression that resolves into the table the new
		/// command will refer to.</param>
		/// <returns>The new command.</returns>
		IDeleteCommand Delete(Func<dynamic, object> table);

		/// <summary>
		/// Creates a new update command for this link.
		/// </summary>
		/// <param name="table">A dynamic lambda expression that resolves into the table the new
		/// command will refer to.</param>
		/// <returns>The new command.</returns>
		IUpdateCommand Update(Func<dynamic, object> table);

		/// <summary>
		/// Factory method invoked to create an enumerator to execute the given enumerable
		/// command.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>An enumerator able to execute de command.</returns>
		IEnumerableExecutor CreateEnumerableExecutor(IEnumerableCommand command);

		/// <summary>
		/// Factory method invoked to create an executor to execute the given scalar command.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>An executor able to execute de command.</returns>
		IScalarExecutor CreateScalarExecutor(IScalarCommand command);
	}
}
