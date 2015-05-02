using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace ErrorControlSystem.Shared.UI.ExceptionMapper
{
    public class StackTraceBlock : TreeViewItem
    {
        public List<StackFrameBlock> ExceptionStackFrames { get; set; }

        public StackTraceBlock(System.Exception exp)
        {
            this.Header = new MessageBlock() { Text = "StackTrace", OpacityMask = Brushes.Aquamarine };

            ExceptionStackFrames = new List<StackFrameBlock>();

            var stackTrace = new StackTrace(exp, true);
            var stackFrames = stackTrace.GetFrames();

            if (stackFrames == null) return;

            foreach (var expStackFrame in stackFrames.Select(frame => new StackFrameBlock() { FrameCodeScope = new CodeScope(frame) }))
            {
                expStackFrame.Clicked += (s, e) => ExceptionStackFrameSelectedChanged(s, e);
                ExceptionStackFrames.Add(expStackFrame);
                Items.Add(expStackFrame);
            }
        }

        public CodeMapEventHandler ExceptionStackFrameSelectedChanged = delegate(object sender, CodeMapEventArgs args) { };
    }
}
