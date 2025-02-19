/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Dev2.Common.Interfaces.WindowsTaskScheduler.Wrappers;
using Microsoft.Win32.TaskScheduler;

namespace Dev2.TaskScheduler.Wrappers
{
    public class Dev2TriggerCollection : ITriggerCollection
    {
        readonly TriggerCollection _nativeInstance;
        readonly ITaskServiceConvertorFactory _taskServiceConvertorFactory;

        public Dev2TriggerCollection(ITaskServiceConvertorFactory taskServiceConvertorFactory,
            TriggerCollection nativeInstance)
        {
            _taskServiceConvertorFactory = taskServiceConvertorFactory;
            _nativeInstance = nativeInstance;
        }

        public IEnumerator<ITrigger> GetEnumerator()
        {
            var en = Instance.GetEnumerator();
            while (en.MoveNext())
            {
                yield return _taskServiceConvertorFactory.CreateTrigger(en.Current);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void IDisposable.Dispose()
        {
            Instance.Dispose();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup
        }

        public ITrigger Add(ITrigger unboundTrigger)
        {
            var instance = unboundTrigger.Instance;
            var trigger = _nativeInstance.Add(instance);
            return _taskServiceConvertorFactory.CreateTrigger(trigger);
        }


        public TriggerCollection Instance => _nativeInstance;
    }
}
