namespace Options.GlobalExtensions
{
	///<summary>
	/// Extensions to <see cref="object"/> to ease work with <see cref="Option{TOption}"/>
	///</summary>
	public static class ObjectExtensions
	{
		///<summary>
		/// Converts <paramref name="value"/> to an <see cref="Option{TOption}"/> of <typeparamref name="TOption"/>
		///</summary>
		///<param name="value">The <typeparamref name="TOption"/> to convert</param>
		///<typeparam name="TOption">The type of <paramref name="value"/></typeparam>
		///<returns>An <see cref="Option{TOption}"/> with an internal type of <typeparamref name="TOption"/></returns>
		public static Option<TOption> AsOption<TOption>(this TOption value)
		{
			return new Option<TOption>(value);
		}
	}
}