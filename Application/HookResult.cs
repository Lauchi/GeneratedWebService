using System.Collections.Generic;

namespace Application
{
    public class HookResult
    {
        private HookResult(List<string> errors)
        {
            Errors = errors;
        }

        public static HookResult OkResult()
        {
            return new HookResult(new List<string>());
        }

        public static HookResult ErrorResult(List<string> errors)
        {
            return new HookResult(errors);
        }

        public List<string> Errors { get; }

        public bool Ok => Errors.Count == 0;
    }
}