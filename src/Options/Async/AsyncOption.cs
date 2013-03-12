using System;
using System.Threading.Tasks;

namespace Options.Async
{
#if !NO_ASYNC
    /// <summary>
    /// Extension and "helper" methods to work with <see cref="Option{TOption}"/> asynchronously
    /// </summary>
    public static class AsyncOption
    {
        /// <summary>
        /// Converts an <see cref="Option{TOption}"/> enclosing a <see cref="Task{TResult}"/> into
        /// a <see cref="Task{TResult}"/> yielding an <see cref="Option{TOption}"/>.
        /// </summary>
        /// <param name="option">An <see cref="Option{TOption}"/> enclosing an asynchronous operation.</param>
        /// <typeparam name="TOption">The type yielded by the <see cref="Task{TResult}"/> of <paramref name="option"/></typeparam>
        /// <returns>A <see cref="Task{TResult}"/> yielding an <see cref="Option{TOption}"/> of <typeparamref name="TOption"/>,.</returns>
        public static async Task<Option<TOption>> InvertAsync<TOption>(this Option<Task<TOption>> option)
        {
            return await option.Handle(async t => Option.Create(await t),
                                       () => Task.FromResult(Option.Create<TOption>()));
        }

        ///<summary>
        ///	Projects the value of an option into a new, asynchronous form.
        ///</summary>
        ///<param name = "option">The optional value to invoke a transform function on.</param>
        ///<param name = "selector">An asynchronous transform function to apply to the optional value.</param>
        ///<typeparam name = "TOption">The type of the value of <paramref name="option"/>.</typeparam>
        ///<typeparam name = "TResult">The type of the value yielded by the <see cref="Task{TResult}"/> returned by <paramref name="selector"/>.</typeparam>
        ///<returns>
        /// A <see cref="Task{TResult}"/> yielding an <see cref = "Option{T}"/> whose value is the result of invoking the transform function on the value of <paramref name = "option"/>.
        ///</returns>
        ///<remarks>
        ///	If <paramref name = "selector" /> is null, the returned <see cref = "Option{TOption}" /> will never contain a value.
        ///</remarks>
        public static Task<Option<TResult>> Select<TOption, TResult>(this Option<TOption> option,
                                                                     Func<TOption, Task<TResult>> selector)
        {
            return OptionExtensions.Select(option, selector).InvertAsync();
        }

        ///<summary>
        ///	Projects the value of an option yielded by a <see cref="Task{TResult}"/> into a new form.
        ///</summary>
        ///<param name = "task">A <see cref="Task{TResult}"/> yielding an <see cref="Option{TOption}"/>.</param>
        ///<param name = "selector">An asynchronous transform function to apply to the optional value.</param>
        ///<typeparam name = "TOption">The type of the value of the <see cref="Option{TOption}"/> yielded by <paramref name="task"/>.</typeparam>
        ///<typeparam name = "TResult">The type of the value yielded by the <see cref="Task{TResult}"/> returned by <paramref name="selector"/>.</typeparam>
        ///<returns>
        /// A <see cref="Task{TResult}"/> yielding an <see cref = "Option{T}"/> whose value is the result of invoking the transform 
        /// function on the value of the <see cref="Option{TOption}"/> yielded by <paramref name = "task"/>.
        ///</returns>
        ///<remarks>
        ///	If either <paramref name="task"/> or <paramref name = "selector" /> is null, the returned <see cref = "Option{TOption}" /> will never contain a value.
        ///</remarks>
        public static async Task<Option<TResult>> Select<TOption, TResult>(this Task<Option<TOption>> task,
                                                                           Func<TOption, Task<TResult>> selector)
        {
            if (task == null)
            {
                return Option.Create<TResult>();
            }
            return await OptionExtensions.Select(await task, selector).InvertAsync();
        }

        ///<summary>
        ///	Projects the value of an option yielded by a <see cref="Task{TResult}"/> into a new form.
        ///</summary>
        ///<param name = "task">A <see cref="Task{TResult}"/> yielding an <see cref="Option{TOption}"/>.</param>
        ///<param name = "selector">An transform function to apply to the optional value.</param>
        ///<typeparam name = "TOption">The type of the value of the <see cref="Option{TOption}"/> yielded by <paramref name="task"/>.</typeparam>
        ///<typeparam name = "TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        ///<returns>
        /// A <see cref="Task{TResult}"/> yielding an <see cref = "Option{T}"/> whose value is the result of invoking the transform 
        /// function on the value of the <see cref="Option{TOption}"/> yielded by <paramref name = "task"/>.
        ///</returns>
        ///<remarks>
        ///	If either <paramref name="task"/> or <paramref name = "selector" /> is null, the returned <see cref = "Option{TOption}" /> will never contain a value.
        ///</remarks>
        public static Task<Option<TResult>> Select<TOption, TResult>(this Task<Option<TOption>> task,
                                                                     Func<TOption, TResult> selector)
        {
            return Select(task, t => Task.FromResult(selector(t)));
        }

        ///<summary>
        ///	Projects the value of an option to an <see cref = "Option{T}" />,  and invokes a result selector function on the value therein.
        ///</summary>
        ///<param name = "option">An optional value to project.</param>
        ///<param name = "selector">An asynchronous transform function to apply to the value of the intermediate option.</param>
        ///<typeparam name = "TOption">The type of the element of <paramref name="option"/>.</typeparam>
        ///<typeparam name = "TResult">The type of the value of the resulting option.</typeparam>
        ///<returns>
        /// An <see cref = "Option{T}" /> whose value is the result of mapping the result value and its corresponding source value to a result value.
        /// If the source or intermediate option has no value, return an empty option of type <typeparamref name="TResult"/>.
        /// </returns>
        public static async Task<Option<TResult>> SelectMany<TOption, TResult>(this Option<TOption> option,
                                                                               Func<TOption, Task<Option<TResult>>>
                                                                                   selector)
        {
            return selector == null
                       ? Option.Create<TResult>()
                       : (await option.Select(selector)).GetValueOrDefault(Option.Create<TResult>());
        }

        ///<summary>
        ///	Projects the value of an option to an <see cref = "Option{T}" />,  and invokes a result selector function on the value therein.
        ///</summary>
        ///<param name = "option">A <see cref="Task{TResult}"/> yielding an optional value to project.</param>
        ///<param name = "selector">An asynchronous transform function to apply to the value of the intermediate option.</param>
        ///<typeparam name = "TOption">The type of the element of <paramref name="option"/>.</typeparam>
        ///<typeparam name = "TResult">The type of the value of the resulting option.</typeparam>
        ///<returns>
        /// An <see cref = "Option{T}" /> whose value is the result of mapping the result value and its corresponding source value to a result value.
        /// If the source or intermediate option has no value, return an empty option of type <typeparamref name="TResult"/>.
        /// </returns>
        public static async Task<Option<TResult>> SelectMany<TOption, TResult>(this Task<Option<TOption>> option,
                                                                               Func<TOption, Task<Option<TResult>>>
                                                                                   selector)
        {
            return selector == null
                       ? Option.Create<TResult>()
                       : (await option.Select(selector)).GetValueOrDefault(Option.Create<TResult>());
        }

        ///<summary>
        ///	Projects the value of an option to an <see cref = "Option{T}" />,  and invokes a result selector function on the value therein.
        ///</summary>
        ///<param name = "option">A <see cref="Task{TResult}"/> yielding an optional value to project.</param>
        ///<param name = "selector">A transform function to apply to the value of the intermediate option.</param>
        ///<typeparam name = "TOption">The type of the element of <paramref name="option"/>.</typeparam>
        ///<typeparam name = "TResult">The type of the value of the resulting option.</typeparam>
        ///<returns>
        /// An <see cref = "Option{T}" /> whose value is the result of mapping the result value and its corresponding source value to a result value.
        /// If the source or intermediate option has no value, return an empty option of type <typeparamref name="TResult"/>.
        /// </returns>
        public static async Task<Option<TResult>> SelectMany<TOption, TResult>(this Task<Option<TOption>> option,
                                                                               Func<TOption, Option<TResult>> selector)
        {
            return selector == null
                       ? Option.Create<TResult>()
                       : (await option.Select(selector)).GetValueOrDefault(Option.Create<TResult>());
        }

        ///<summary>
        ///	Projects the value of an option to an <see cref = "Option{T}" />, intersects the source and resulting options, and invokes a result selector function on the value therein.
        ///</summary>
        ///<param name = "option">An optional value to project.</param>
        ///<param name = "optionSelector">An asynchronous transform function to apply to the value of the input option.</param>
        ///<param name = "resultSelector">A transform function to apply to the value of the intermediate option.</param>
        ///<typeparam name = "TSource">The type of the elements of source.</typeparam>
        ///<typeparam name = "TIntermediate">The type of the intermediate value returned by optionSelector.</typeparam>
        ///<typeparam name = "TResult">The type of the value of the resulting option.</typeparam>
        ///<returns>An <see cref = "Option{T}" /> whose value is the result of invoking the transform function optionSelector on the value of source and then mapping the result value and its corresponding source value to a result value. If the source or intermediate option has no value, return an empty option of type <typeparamref name="TResult"/>.</returns>
        ///<exception cref = "ArgumentNullException"><paramref name="optionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        public static Task<Option<TResult>> SelectMany<TSource, TIntermediate, TResult>(this Option<TSource> option,
                                                                                        Func
                                                                                            <TSource,
                                                                                            Task<Option<TIntermediate>>>
                                                                                            optionSelector,
                                                                                        Func
                                                                                            <TSource, TIntermediate,
                                                                                            TResult> resultSelector)
        {
            if (optionSelector == null)
            {
                throw new ArgumentNullException("optionSelector");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            return
                option.Intersect(option.SelectMany(optionSelector))
                      .Select(pair => resultSelector(pair.Item1, pair.Item2));
        }

        ///<summary>
        ///	Projects the value of an option to an <see cref = "Option{T}" />, intersects the source and resulting options, and invokes a result selector function on the value therein.
        ///</summary>
        ///<param name = "option">An optional value to project.</param>
        ///<param name = "optionSelector">A transform function to apply to the value of the input option.</param>
        ///<param name = "resultSelector">An asynchronous transform function to apply to the value of the intermediate option.</param>
        ///<typeparam name = "TSource">The type of the elements of source.</typeparam>
        ///<typeparam name = "TIntermediate">The type of the intermediate value returned by optionSelector.</typeparam>
        ///<typeparam name = "TResult">The type of the value of the resulting option.</typeparam>
        ///<returns>An <see cref = "Option{T}" /> whose value is the result of invoking the transform function optionSelector on the value of source and then mapping the result value and its corresponding source value to a result value. If the source or intermediate option has no value, return an empty option of type <typeparamref name="TResult"/>.</returns>
        ///<exception cref = "ArgumentNullException"><paramref name="optionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        public static Task<Option<TResult>> SelectMany<TSource, TIntermediate, TResult>(this Option<TSource> option,
                                                                                        Func
                                                                                            <TSource,
                                                                                            Option<TIntermediate>>
                                                                                            optionSelector,
                                                                                        Func
                                                                                            <TSource, TIntermediate,
                                                                                            Task<TResult>>
                                                                                            resultSelector)
        {
            if (optionSelector == null)
            {
                throw new ArgumentNullException("optionSelector");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            return
                option.Intersect(option.SelectMany(optionSelector))
                      .Select(pair => resultSelector(pair.Item1, pair.Item2));
        }

        ///<summary>
        ///	Projects the value of an option to an <see cref = "Option{T}" />, intersects the source and resulting options, and invokes a result selector function on the value therein.
        ///</summary>
        ///<param name = "option">An optional value to project.</param>
        ///<param name = "optionSelector">An asynchronous transform function to apply to the value of the input option.</param>
        ///<param name = "resultSelector">An asynchronous transform function to apply to the value of the intermediate option.</param>
        ///<typeparam name = "TSource">The type of the elements of source.</typeparam>
        ///<typeparam name = "TIntermediate">The type of the intermediate value returned by optionSelector.</typeparam>
        ///<typeparam name = "TResult">The type of the value of the resulting option.</typeparam>
        ///<returns>An <see cref = "Option{T}" /> whose value is the result of invoking the transform function optionSelector on the value of source and then mapping the result value and its corresponding source value to a result value. If the source or intermediate option has no value, return an empty option of type <typeparamref name="TResult"/>.</returns>
        ///<exception cref = "ArgumentNullException"><paramref name="optionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        public static Task<Option<TResult>> SelectMany<TSource, TIntermediate, TResult>(this Option<TSource> option,
                                                                                        Func
                                                                                            <TSource,
                                                                                            Task<Option<TIntermediate>>>
                                                                                            optionSelector,
                                                                                        Func
                                                                                            <TSource, TIntermediate,
                                                                                            Task<TResult>>
                                                                                            resultSelector)
        {
            if (optionSelector == null)
            {
                throw new ArgumentNullException("optionSelector");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            return
                option.Intersect(option.SelectMany(optionSelector))
                      .Select(pair => resultSelector(pair.Item1, pair.Item2));
        }

        ///<summary>
        ///	Projects the value of an option to an <see cref = "Option{T}" />, intersects the source and resulting options, and invokes a result selector function on the value therein.
        ///</summary>
        ///<param name = "task">A <see cref="Task{TResult}"/> yielding an optional value to project.</param>
        ///<param name = "optionSelector">An transform function to apply to the value of the input option.</param>
        ///<param name = "resultSelector">A transform function to apply to the value of the intermediate option.</param>
        ///<typeparam name = "TSource">The type of the elements of source.</typeparam>
        ///<typeparam name = "TIntermediate">The type of the intermediate value returned by optionSelector.</typeparam>
        ///<typeparam name = "TResult">The type of the value of the resulting option.</typeparam>
        ///<returns>An <see cref = "Option{T}" /> whose value is the result of invoking the transform function optionSelector on the value of source and then mapping the result value and its corresponding source value to a result value. If the source or intermediate option has no value, return an empty option of type <typeparamref name="TResult"/>.</returns>
        ///<exception cref = "ArgumentNullException"><paramref name="optionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        public static Task<Option<TResult>> SelectMany<TSource, TIntermediate, TResult>(this Task<Option<TSource>> task,
                                                                                        Func
                                                                                            <TSource,
                                                                                            Option<TIntermediate>>
                                                                                            optionSelector,
                                                                                        Func
                                                                                            <TSource, TIntermediate,
                                                                                            TResult> resultSelector)
        {
            if (optionSelector == null)
            {
                throw new ArgumentNullException("optionSelector");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            return task.Intersect(task.SelectMany(optionSelector))
                       .Select(pair => resultSelector(pair.Item1, pair.Item2));
        }

        ///<summary>
        ///	Projects the value of an option to an <see cref = "Option{T}" />, intersects the source and resulting options, and invokes a result selector function on the value therein.
        ///</summary>
        ///<param name = "task">A <see cref="Task{TResult}"/> yielding an optional value to project.</param>
        ///<param name = "optionSelector">An asynchronous transform function to apply to the value of the input option.</param>
        ///<param name = "resultSelector">A transform function to apply to the value of the intermediate option.</param>
        ///<typeparam name = "TSource">The type of the elements of source.</typeparam>
        ///<typeparam name = "TIntermediate">The type of the intermediate value returned by optionSelector.</typeparam>
        ///<typeparam name = "TResult">The type of the value of the resulting option.</typeparam>
        ///<returns>An <see cref = "Option{T}" /> whose value is the result of invoking the transform function optionSelector on the value of source and then mapping the result value and its corresponding source value to a result value. If the source or intermediate option has no value, return an empty option of type <typeparamref name="TResult"/>.</returns>
        ///<exception cref = "ArgumentNullException"><paramref name="optionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        public static Task<Option<TResult>> SelectMany<TSource, TIntermediate, TResult>(this Task<Option<TSource>> task,
                                                                                        Func
                                                                                            <TSource,
                                                                                            Task<Option<TIntermediate>>>
                                                                                            optionSelector,
                                                                                        Func
                                                                                            <TSource, TIntermediate,
                                                                                            TResult> resultSelector)
        {
            if (optionSelector == null)
            {
                throw new ArgumentNullException("optionSelector");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            return task.Intersect(task.SelectMany(optionSelector))
                       .Select(pair => resultSelector(pair.Item1, pair.Item2));
        }

        ///<summary>
        ///	Projects the value of an option to an <see cref = "Option{T}" />, intersects the source and resulting options, and invokes a result selector function on the value therein.
        ///</summary>
        ///<param name = "task">A <see cref="Task{TResult}"/> yielding an optional value to project.</param>
        ///<param name = "optionSelector">A transform function to apply to the value of the input option.</param>
        ///<param name = "resultSelector">An asynchronous transform function to apply to the value of the intermediate option.</param>
        ///<typeparam name = "TSource">The type of the elements of source.</typeparam>
        ///<typeparam name = "TIntermediate">The type of the intermediate value returned by optionSelector.</typeparam>
        ///<typeparam name = "TResult">The type of the value of the resulting option.</typeparam>
        ///<returns>An <see cref = "Option{T}" /> whose value is the result of invoking the transform function optionSelector on the value of source and then mapping the result value and its corresponding source value to a result value. If the source or intermediate option has no value, return an empty option of type <typeparamref name="TResult"/>.</returns>
        ///<exception cref = "ArgumentNullException"><paramref name="optionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        public static Task<Option<TResult>> SelectMany<TSource, TIntermediate, TResult>(this Task<Option<TSource>> task,
                                                                                        Func
                                                                                            <TSource,
                                                                                            Option<TIntermediate>>
                                                                                            optionSelector,
                                                                                        Func
                                                                                            <TSource, TIntermediate,
                                                                                            Task<TResult>>
                                                                                            resultSelector)
        {
            if (optionSelector == null)
            {
                throw new ArgumentNullException("optionSelector");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            return task.Intersect(task.SelectMany(optionSelector))
                       .Select(pair => resultSelector(pair.Item1, pair.Item2));
        }

        ///<summary>
        ///	Projects the value of an option to an <see cref = "Option{T}" />, intersects the source and resulting options, and invokes a result selector function on the value therein.
        ///</summary>
        ///<param name = "task">A <see cref="Task{TResult}"/> yielding an optional value to project.</param>
        ///<param name = "optionSelector">An asynchronous transform function to apply to the value of the input option.</param>
        ///<param name = "resultSelector">An asynchronous transform function to apply to the value of the intermediate option.</param>
        ///<typeparam name = "TSource">The type of the elements of source.</typeparam>
        ///<typeparam name = "TIntermediate">The type of the intermediate value returned by optionSelector.</typeparam>
        ///<typeparam name = "TResult">The type of the value of the resulting option.</typeparam>
        ///<returns>An <see cref = "Option{T}" /> whose value is the result of invoking the transform function optionSelector on the value of source and then mapping the result value and its corresponding source value to a result value. If the source or intermediate option has no value, return an empty option of type <typeparamref name="TResult"/>.</returns>
        ///<exception cref = "ArgumentNullException"><paramref name="optionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        public static Task<Option<TResult>> SelectMany<TSource, TIntermediate, TResult>(this Task<Option<TSource>> task,
                                                                                        Func
                                                                                            <TSource,
                                                                                            Task<Option<TIntermediate>>>
                                                                                            optionSelector,
                                                                                        Func
                                                                                            <TSource, TIntermediate,
                                                                                            Task<TResult>>
                                                                                            resultSelector)
        {
            if (optionSelector == null)
            {
                throw new ArgumentNullException("optionSelector");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            return task.Intersect(task.SelectMany(optionSelector))
                       .Select(pair => resultSelector(pair.Item1, pair.Item2));
        }

        /// <summary>
        /// Filters an option based on a predicate.
        /// </summary>
        /// <param name="option">The <see cref="Option{TOption}"/> to filter.</param>
        /// <param name="predicate">An asynchronous <see cref="Predicate{TResult}"/> used to filter the option</param>
        /// <typeparam name="TSource">The type of the value contained by <paramref name="option"/></typeparam>
        /// <returns><paramref name="option"/>, if <paramref name="option"/> has a value and <paramref name="predicate"/> returns true; an empty <see cref="Option{TOption}"/>, otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
        public static async Task<Option<TSource>> Where<TSource>(this Option<TSource> option,
                                                                 Func<TSource, Task<bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            return (await option.Handle(predicate, () => Task.FromResult(false)))
                       ? option
                       : Option.Create<TSource>();
        }

        /// <summary>
        /// Filters an option based on a predicate.
        /// </summary>
        /// <param name="task">A <see cref="Task{TResult}"/> yielding the <see cref="Option{TOption}"/> to filter.</param>
        /// <param name="predicate">An asynchronous <see cref="Predicate{TResult}"/> used to filter the option</param>
        /// <typeparam name="TSource">The type of the value contained by <paramref name="task"/></typeparam>
        /// <returns><paramref name="task"/>, if <paramref name="task"/> has a value and <paramref name="predicate"/> returns true; an empty <see cref="Option{TOption}"/>, otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
        public static async Task<Option<TSource>> Where<TSource>(this Task<Option<TSource>> task,
                                                                 Func<TSource, Task<bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            var option = await task;

            return (await option.Select(predicate)).Handle(b => b, () => false)
                       ? option
                       : Option.Create<TSource>();
        }

        /// <summary>
        /// Filters an option based on a predicate.
        /// </summary>
        /// <param name="task">A <see cref="Task{TResult}"/> yielding the <see cref="Option{TOption}"/> to filter.</param>
        /// <param name="predicate">A <see cref="Predicate{TResult}"/> used to filter the option</param>
        /// <typeparam name="TSource">The type of the value contained by <paramref name="task"/></typeparam>
        /// <returns><paramref name="task"/>, if <paramref name="task"/> has a value and <paramref name="predicate"/> returns true; an empty <see cref="Option{TOption}"/>, otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
        public static async Task<Option<TSource>> Where<TSource>(this Task<Option<TSource>> task,
                                                                 Func<TSource, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            var option = await task;

            return option.Select(predicate).Handle(b => b, () => false)
                       ? option
                       : Option.Create<TSource>();
        }

        ///<summary>
        ///	Retrieves the value contained in <paramref name = "task" /> or the default value of the type.
        ///</summary>
        ///<param name = "task">A <see cref="Task{TResult}"/> yielding <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> internal type</param>
        ///<typeparam name = "TOption">The internal type of <paramref name = "task" /></typeparam>
        ///<returns>A <typeparamref name = "TOption" /> value. The internal value of <paramref name = "task" /> or the default value of <typeparamref name="TOption"/></returns>
        public static Task<TOption> DefaultIfEmpty<TOption>(this Task<Option<TOption>> task)
            where TOption : struct
        {
            return task.DefaultIfEmpty(() => default(TOption));
        }

        /// <summary>
        /// 	Retrieves the value contained in <paramref name = "task" /> or the value returned from <paramref name="createDefault"/>.
        /// </summary>
        /// <param name = "task">A <see cref="Task{TResult}"/> yielding <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> internal type</param>
        /// <param name="createDefault">A function called to generate a default value.</param>
        /// <typeparam name = "TOption">The internal type of <paramref name = "task" /></typeparam>
        /// <returns>A <typeparamref name = "TOption" /> value.</returns>
        public static async Task<TOption> DefaultIfEmpty<TOption>(this Task<Option<TOption>> task,
                                                                  Func<TOption> createDefault)
        {
            return (await task).Handle(t => t, createDefault);
        }

        /// <summary>
        /// 	Retrieves the value contained in <paramref name = "task" /> or the value returned from <paramref name="createDefault"/>.
        /// </summary>
        /// <param name = "task">A <see cref="Task{TResult}"/> yielding <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> internal type</param>
        /// <param name="createDefault">An asynchronous function called to generate a default value.</param>
        /// <typeparam name = "TOption">The internal type of <paramref name = "task" /></typeparam>
        /// <returns>A <typeparamref name = "TOption" /> value.</returns>
        public static async Task<TOption> DefaultIfEmpty<TOption>(this Task<Option<TOption>> task,
                                                                  Func<Task<TOption>> createDefault)
        {
            return await (await task).Handle(Task.FromResult, createDefault);
        }

        ///<summary>
        ///	Retrieves the value contained in <paramref name = "task" /> or throws an exception.
        ///</summary>
        /// <param name = "task">A <see cref="Task{TResult}"/> yielding <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> internal type</param>
        ///<param name = "createException">A function used to create an <typeparamref name = "TException" /> to throw</param>
        ///<typeparam name = "TOption">The internal type of <paramref name = "task" /></typeparam>
        ///<typeparam name = "TException">The type of exception to throw</typeparam>
        ///<returns>The internal value of <paramref name = "task" /></returns>
        ///<exception><paramref name = "task" /> contains no value</exception>
        ///<exception cref = "ArgumentNullException"><paramref name = "createException" /> is null</exception>
        public static async Task<TOption> ThrowIfEmpty<TOption, TException>(this Task<Option<TOption>> task,
                                                                            Func<TException> createException)
            where TException : Exception
        {
            if (createException == null)
            {
                throw new ArgumentNullException("createException");
            }
            return (await task).Handle(v => v, () => { throw createException(); });
        }

        ///<summary>
        ///	Retrieves the value contained in <paramref name = "task" /> or throws an exception.
        ///</summary>
        /// <param name = "task">A <see cref="Task{TResult}"/> yielding <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> internal type</param>
        ///<typeparam name = "TOption">The internal type of <paramref name = "task" /></typeparam>
        ///<returns>The internal value of <paramref name = "task" /></returns>
        ///<exception><paramref name = "task" /> contains no value</exception>
        public static Task<TOption> ThrowIfEmpty<TOption>(this Task<Option<TOption>> task)
        {
            return task.ThrowIfEmpty(() => new NoneException());
        }

        ///<summary>
        ///	Returns a <see cref = "bool" /> indicating whether <paramref name = "task" /> contains a value.
        ///</summary>
        /// <param name = "task">A <see cref="Task{TResult}"/> yielding <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> internal type</param>
        ///<typeparam name = "TOption">The internal type of <paramref name = "task" /></typeparam>
        ///<returns>True, if <paramref name = "task" /> contains a value; false, if not.</returns>
        public static Task<bool> ContainsValue<TOption>(this Task<Option<TOption>> task)
        {
            return task.Select(v => true).DefaultIfEmpty(() => false);
        }

        ///<summary>
        ///	Converts an <see cref = "Option{TOption}" /> to a nullable value type.
        ///</summary>
        ///<param name = "task">The <see cref = "Option{TOption}" /> to convert to a nullable <typeparamref name = "TOption" /></param>
        ///<typeparam name = "TOption">The internal type of <paramref name = "task" /></typeparam>
        ///<returns>A nullable <typeparamref name = "TOption" /> which will contain a value if <paramref name = "task" /> does.</returns>
        public static Task<TOption?> AsNullable<TOption>(this Task<Option<TOption>> task)
            where TOption : struct
        {
            return task.Select(v => (TOption?)v).DefaultIfEmpty(() => (TOption?)null);
        }

        ///<summary>
        ///	Converts an <see cref = "Option{TOption}" /> to a reference type.
        ///</summary>
        ///<param name = "task">The <see cref = "Option{TOption}" /> to convert to a nullable <typeparamref name = "TOption" /></param>
        ///<typeparam name = "TOption">The internal type of <paramref name = "task" /></typeparam>
        ///<returns>A(n) <typeparamref name = "TOption" /> which will be null if <paramref name = "task" /> does not contain a value.</returns>
        public static Task<TOption> AsUnprotected<TOption>(this Task<Option<TOption>> task)
            where TOption : class
        {
            return task.DefaultIfEmpty(() => (TOption)null);
        }

        /// <summary>
        /// Runs one of the given actions based on whether the given <see cref="Option{TOption}"/> has a value.
        /// </summary>
        ///<typeparam name = "TOption">The internal type of <paramref name = "task" /></typeparam>
        /// <param name="task">The <see cref="Option{TOption}"/> to act on.</param>
        /// <param name="ifSome">The action that is run when the option has a value.</param>
        /// <param name="ifNone">The action that is run when the option has no value.</param>
        public static async Task Act<TOption>(this Task<Option<TOption>> task, Action<TOption> ifSome, Action ifNone)
        {
            if (ifSome == null)
            {
                throw new ArgumentNullException("ifSome");
            }
            if (ifNone == null)
            {
                throw new ArgumentNullException("ifNone");
            }
            if (await task.ContainsValue())
            {
                ifSome(await task.ThrowIfEmpty());
            }
            else
            {
                ifNone();
            }
        }

        /// <summary>
        /// Runs one of the given actions based on whether the given <see cref="Option{TOption}"/> has a value.
        /// </summary>
        ///<typeparam name = "TOption">The internal type of <paramref name = "option" /></typeparam>
        /// <param name="option">The <see cref="Option{TOption}"/> to act on.</param>
        /// <param name="ifSome">The action that is run when the option has a value.</param>
        /// <param name="ifNone">The action that is run when the option has no value.</param>
        public static Task Act<TOption>(this Option<TOption> option, Func<TOption, Task> ifSome, Func<Task> ifNone)
        {
            if (ifSome == null)
            {
                throw new ArgumentNullException("ifSome");
            }
            if (ifNone == null)
            {
                throw new ArgumentNullException("ifNone");
            }
            return option.Handle(ifSome, ifNone);
        }

        /// <summary>
        /// Runs one of the given actions based on whether the given <see cref="Option{TOption}"/> has a value.
        /// </summary>
        ///<typeparam name = "TOption">The internal type of <paramref name = "task" /></typeparam>
        /// <param name="task">The <see cref="Option{TOption}"/> to act on.</param>
        /// <param name="ifSome">The action that is run when the option has a value.</param>
        /// <param name="ifNone">The action that is run when the option has no value.</param>
        public static async Task Act<TOption>(this Task<Option<TOption>> task,
                                              Func<TOption, Task> ifSome,
                                              Func<Task> ifNone)
        {
            if (ifSome == null)
            {
                throw new ArgumentNullException("ifSome");
            }
            if (ifNone == null)
            {
                throw new ArgumentNullException("ifNone");
            }
            await (await task).Act(ifSome, ifNone);
        }

        /// <summary>
        /// Creates a <see cref="Task{TResult}"/> yielding an <see cref="Option{TOption}"/> of <typeparamref name="TOption"/>.
        /// </summary>
        /// <param name="task">A <see cref="Task{TResult}"/> which yields a <typeparamref name="TOption"/></param>
        /// <typeparam name="TOption">The type yielded by <paramref name="task"/></typeparam>
        /// <returns>A <see cref="Task{TResult}"/> yielding an <see cref="Option{TOption}"/> of <typeparamref name="TOption"/>.</returns>
        public static Task<Option<TOption>> Create<TOption>(Task<TOption> task)
        {
            return task == null ? Task.FromResult(new Option<TOption>()) : new Option<Task<TOption>>(task).InvertAsync();
        }

        /// <summary>
        /// Creates a <see cref="Task{TResult}"/> yielding an <see cref="Option{TOption}"/> of <typeparamref name="TOption"/>.
        /// </summary>
        /// <param name="value">A <typeparamref name="TOption"/> value</param>
        /// <typeparam name="TOption">The type of <paramref name="value"/></typeparam>
        /// <returns>A <see cref="Task{TResult}"/> yielding an <see cref="Option{TOption}"/> of <typeparamref name="TOption"/>.</returns>
        public static Task<Option<TOption>> Create<TOption>(TOption value)
        {
            return Task.FromResult(ReferenceEquals(value, null) ? new Option<TOption>() : new Option<TOption>(value));
        }

        /// <summary>
        /// Creates a <see cref="Task{TResult}"/> yielding an empty <see cref="Option{TOption}"/> of <typeparamref name="TOption"/>.
        /// </summary>
        /// <typeparam name="TOption">The type protected by the empty <see cref="Option{TOption}"/></typeparam>
        /// <returns>A <see cref="Task{TResult}"/> yielding an <see cref="Option{TOption}"/> of <typeparamref name="TOption"/>.</returns>
        public static Task<Option<TOption>> Create<TOption>()
        {
            return Task.FromResult(new Option<TOption>());
        }

        /// <summary>
        /// Creates a <see cref="Task{TResult}"/> yielding an <see cref="Option{TOption}"/> of <typeparamref name="TOption"/>.
        /// </summary>
        /// <param name="task">A <see cref="Task{TResult}"/> which yields a <see cref="Nullable{T}"/> of <typeparamref name="TOption"/></param>
        /// <typeparam name="TOption">The type yielded by <paramref name="task"/></typeparam>
        /// <returns>A <see cref="Task{TResult}"/> yielding an <see cref="Option{TOption}"/> of <typeparamref name="TOption"/>.</returns>
        public static Task<Option<TOption>> Create<TOption>(Task<TOption?> task)
            where TOption : struct
        {
            return from value in Option.Create(task).InvertAsync()
                   where value.HasValue
                   // ReSharper disable PossibleInvalidOperationException
                   // Nope
                   select value.Value;
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Creates a <see cref="Task{TResult}"/> yielding an <see cref="Option{TOption}"/> of <typeparamref name="TOption"/>.
        /// </summary>
        /// <param name="value">A <see cref="Nullable{T}"/> of <typeparamref name="TOption"/> value</param>
        /// <typeparam name="TOption">The type of <paramref name="value"/></typeparam>
        /// <returns>A <see cref="Task{TResult}"/> yielding an <see cref="Option{TOption}"/> of <typeparamref name="TOption"/>.</returns>
        public static Task<Option<TOption>> Create<TOption>(TOption? value)
            where TOption : struct
        {
            return value.HasValue
                ? Task.FromResult(new Option<TOption>(value.Value))
                : Task.FromResult(new Option<TOption>());
        }

        private static Task<Option<Tuple<T1, T2>>> Intersect<T1, T2>(this Option<T1> first, Task<Option<T2>> second)
        {
            return first.Handle(v => second.Select(v2 => Tuple.Create(v, v2)),
                                () => Task.FromResult(new Option<Tuple<T1, T2>>()));
        }

        private static async Task<Option<Tuple<T1, T2>>> Intersect<T1, T2>(this Task<Option<T1>> first,
                                                                           Task<Option<T2>> second)
        {
            return await (await first).Handle(v => second.Select(v2 => Tuple.Create(v, v2)),
                                              () => Task.FromResult(new Option<Tuple<T1, T2>>()));
        }
    }
#endif
}