﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CatLib._3rd.SharpCompress.Compressors.LZMA
{
    internal static class Log
    {
        private static readonly Stack<string> _indent = new Stack<string>();
        private static bool _needsIndent = true;

        static Log()
        {
            _indent.Push("");
        }

        public static void PushIndent(string indent = "  ")
        {
            _indent.Push(_indent.Peek() + indent);
        }

        public static void PopIndent()
        {
            if (_indent.Count == 1)
            {
                throw new InvalidOperationException();
            }

            _indent.Pop();
        }

        private static void EnsureIndent()
        {
            if (_needsIndent)
            {
                _needsIndent = false;
            }
        }

        public static void Write(object value)
        {
            EnsureIndent();
        }

        public static void Write(string text)
        {
            EnsureIndent();
        }

        public static void Write(string format, params object[] args)
        {
            EnsureIndent();
        }

        public static void WriteLine()
        {
            Debug.WriteLine("");
            _needsIndent = true;
        }

        public static void WriteLine(object value)
        {
            EnsureIndent();
            Debug.WriteLine(value);
            _needsIndent = true;
        }

        public static void WriteLine(string text)
        {
            EnsureIndent();
            Debug.WriteLine(text);
            _needsIndent = true;
        }

        public static void WriteLine(string format, params object[] args)
        {
            EnsureIndent();
            Debug.WriteLine(string.Format(format, args));
            _needsIndent = true;
        }
    }
}