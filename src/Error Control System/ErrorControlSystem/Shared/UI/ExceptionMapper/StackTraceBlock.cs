using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;

namespace ErrorControlSystem.Shared.UI.ExceptionMapper
{
    public class StackTraceBlock : TreeViewItem
    {
        public List<StackFrameBlock> ExceptionStackFrames { get; set; }

        public StackTraceBlock(System.Exception exp)
        {
            this.Header = new HeaderBlock() { Text = "StackTrace" };

            ExceptionStackFrames = new List<StackFrameBlock>();

            var stackTrace = new StackTrace(exp, true);
            var stackFrames = stackTrace.GetFrames();

            if (stackFrames != null)
                foreach (var frame in stackFrames)
                {
                    var expStackFrame = new StackFrameBlock() { FrameCodeScope = new CodeScope(frame) };

                    expStackFrame.Clicked += (s, e) => ExceptionStackFrameSelectedChanged(s, e);
                    ExceptionStackFrames.Add(expStackFrame);
                    Items.Add(expStackFrame);
                }

            IsExpanded = true;
        }

        public CodeMapEventHandler ExceptionStackFrameSelectedChanged = delegate(object sender, CodeMapEventArgs args) { };
    }
}
