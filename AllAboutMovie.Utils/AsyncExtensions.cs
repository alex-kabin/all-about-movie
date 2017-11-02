using System;
using System.ComponentModel;
using System.Threading;

namespace AllAboutMovie.Utils
{
	public static class AsyncExtensions
	{
		public static IAsyncResult InvokeAsync<TArg, TResult>(
			this Func<TArg, CancellationToken, TResult> operation,
			TArg arg,
			Action<TResult, Exception> callback,
			CancellationToken cancelToken)
		{
			var asyncOperation = AsyncOperationManager.CreateOperation(null);
			return operation.BeginInvoke(arg,
			                             cancelToken,
			                             delegate(IAsyncResult ar)
			                             	{
			                             		TResult result = default(TResult);
			                             		Exception error = null;
			                             		try
			                             		{
			                             			result = operation.EndInvoke(ar);
			                             		}
			                             		catch (Exception ex)
			                             		{
			                             			error = ex;
			                             		}

			                             		asyncOperation.PostOperationCompleted(
			                             			delegate
			                             				{
			                             					if (!cancelToken.IsCancellationRequested)
			                             						callback(result, error);
			                             				},
			                             			null);
			                             	},
			                             null);
		}
	}
}