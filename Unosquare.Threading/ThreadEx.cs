using System;
using System.Threading;

namespace Unosquare.Threading
{
    /// <summary>
    /// 线程增强类，简化线程的使用
    /// </summary>
    public class ThreadEx
    {
        /// <summary>
        /// 线程中发生错误(全局)
        /// </summary>
        public static event EventHandler<ThreadEventArgs> OnErrOccurred;

        /// <summary>
        /// 异步执行某个动作
        /// </summary>
        /// <param name="action">动作</param>
        /// <returns>运行的线程</returns>
        public static Thread Run(ThreadStart action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            ThreadStart innerAction = () =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    if (OnErrOccurred != null)
                    {
                        OnErrOccurred.Invoke(null, new ThreadEventArgs(ex));
                    }
                    else
                    {
                        throw;
                    }
                }
            };

            Thread thread = new Thread(innerAction);
            thread.IsBackground = true;
            thread.Start();
            return thread;
        }

        /// <summary>
        /// 异步执行某个动作
        /// </summary>
        /// <param name="action">动作</param>
        /// <param name="parameter">参数</param>
        /// <returns>运行的线程</returns>
        public static Thread Run(ParameterizedThreadStart action, object parameter)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            ParameterizedThreadStart innerAction = param =>
            {
                try
                {
                    action(param);
                }
                catch (Exception ex)
                {
                    if (OnErrOccurred != null)
                    {
                        OnErrOccurred.Invoke(null, new ThreadEventArgs(ex));
                    }
                    else
                    {
                        throw;
                    }
                }
            };

            Thread thread = new Thread(action);
            thread.IsBackground = true;
            if (parameter != null)
            {
                thread.Start(parameter);
            }
            
            return thread;
        }

        /// <summary>
        /// 线程暂停
        /// </summary>
        /// <param name="dueTime">暂停时间</param>
        public static void Delay(TimeSpan dueTime)
        {
            Thread.Sleep((int)dueTime.TotalMilliseconds);
        }
    }
}
