using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace glua_scraper.provider
{
    public class TypescriptProvider : IProvider
    {
        public string GetName()
        {
            return "Typescript";
        }

        public void OnStart()
        {

        }

        public void OnFinish()
        {

        }

        public void SaveHooks(Dictionary<string, List<Hook>> hooks)
        {
            foreach (KeyValuePair<string, List<Hook>> hookList in hooks)
            {
                Console.WriteLine(GenerateHookClass(hookList.Key, hookList.Value));
            }
        }

        public string GenerateHookClass(string hookClass, List<Hook> hooks)
        {
            string result = $"interface {hookClass} {{\n";
            hooks.ForEach(hook => result += GenerateHook(hook));
            result += "}";

            return result;
        }

        public string GenerateHookClass(string hookClass, List<Hook> hooks, List<string> aliases)
        {
            string result = GenerateHookClass(hookClass, hooks);
            aliases.ForEach(alias => result += $"declare const {alias}: {hookClass};\n");
            return result;
        }

        public string GenerateHook(Hook hook)
        {
            string indent = new String(' ', 4);
            string argString = string.Join(", ", hook.Args.Select(arg => arg.Name + ": " + arg.Type));
            string retString = "";
            bool tupleReturn = false;
            if (hook.ReturnValues.Count == 1)
            {
                retString = hook.ReturnValues[0].Type;
            } else
            {
                string retList = string.Join(", ", hook.ReturnValues.Select(ret => ret.Type));
                retString = $"[{retList}]";
                tupleReturn = true;
            }


            string result = "";
            result += indent + $"/**";
            result += indent + $" * {hook.Description}";
            if (tupleReturn)
            {
                result += indent + " * !TupleReturn";
            }
            result += indent + $" */";
            result += indent + $"{hook.Name}({argString}): {retString}";

            return result;
        }

        public void SaveGlobals(Dictionary<string, List<Function>> globals)
        {

        }

        public void SaveLibFuncs(Dictionary<string, List<Function>> libFuncs)
        {

        }

        public void SaveClassFuncs(Dictionary<string, List<Function>> classFuncs)
        {

        }

        public void SavePanelFuncs(Dictionary<string, List<Function>> panelFuncs)
        {

        }
    }
}