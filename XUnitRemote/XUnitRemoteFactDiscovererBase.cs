using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace XUnitRemote
{
    public abstract class XUnitRemoteFactDiscovererBase : IXunitTestCaseDiscoverer
    {
        private readonly IMessageSink _DiagnosticMessageSink;
        private readonly FactDiscoverer _FactDiscoverer;

        protected XUnitRemoteFactDiscovererBase(IMessageSink diagnosticMessageSink)
        {
            _DiagnosticMessageSink = diagnosticMessageSink;
            _FactDiscoverer = new FactDiscoverer(diagnosticMessageSink);
        }

        /// <summary>
        /// This is the Id the WCF process will use to namespace the communication channel. If you 
        /// wish to unit test more than one remote process at a time the xunit remote engine needs
        /// to give each a seperate name.
        /// </summary>
        protected abstract string Id { get; }

        /// <summary>
        /// The full path to the executable to start the remote process
        /// </summary>
        protected abstract string ExePath { get; }

        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            if (!Process.GetCurrentProcess().ProcessName.Equals(Path.GetFileNameWithoutExtension(ExePath), StringComparison.OrdinalIgnoreCase))
            {
                var id = Id;
                var exePath = ExePath;

                yield return new XUnitRemoteTestCase(_DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod, id, exePath);
            }
            else
            {
                foreach (var testCase in _FactDiscoverer.Discover(discoveryOptions, testMethod, factAttribute))
                {
                    yield return testCase;
                }
            }
        }
    }

    public abstract class XUnitRemoteTheoryDiscovererBase : IXunitTestCaseDiscoverer
    {
        private readonly IMessageSink _DiagnosticMessageSink;
        private readonly TheoryDiscoverer _TheoryDiscoverer;

        protected XUnitRemoteTheoryDiscovererBase(IMessageSink diagnosticMessageSink)
        {
            _DiagnosticMessageSink = diagnosticMessageSink;
            _TheoryDiscoverer = new TheoryDiscoverer(diagnosticMessageSink);
        }

        /// <summary>
        /// This is the Id the WCF process will use to namespace the communication channel. If you 
        /// wish to unit test more than one remote process at a time the xunit remote engine needs
        /// to give each a seperate name.
        /// </summary>
        protected abstract string Id { get; }

        /// <summary>
        /// The full path to the executable to start the remote process
        /// </summary>
        protected abstract string ExePath { get; }

        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            if (!Process.GetCurrentProcess().ProcessName.Equals(Path.GetFileNameWithoutExtension(ExePath), StringComparison.OrdinalIgnoreCase))
            {
                var id = Id;
                var exePath = ExePath;

                yield return new XUnitRemoteTestCase(_DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod, id, exePath);
            }
            else
            {
                foreach (var testCase in _TheoryDiscoverer.Discover(discoveryOptions, testMethod, factAttribute))
                {
                    yield return testCase;
                }
            }
        }
    }
}