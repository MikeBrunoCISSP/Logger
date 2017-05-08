using System;
using System.IO;
using System.Diagnostics;

namespace Logger
{
    public enum Level
    {
        _NONE = -1,
        _CRITICAL = 0,
        _ERROR = 1,
        _WARNING = 2,
        _INFO = 3,
        _DEBUG = 4,
        _MASSIVE = 5
    }

    public enum InitMode
    {
        _APPEND = 0,
        _OVERWRITE = 1
    }

    public class Log
    {
        private static string INDENT = "                                     ";
        private static StreamWriter stream;

        private string file;
        private Level level;

        private bool DEBUG_writeToOutputWindow,
                     loggingDisabled;

        public Log(string fileName)
        {
            file = fileName;
            level = Level._INFO;
            DEBUG_writeToOutputWindow = false;
            loggingDisabled = false;
        }

        public Log(string fileName, InitMode mode)
        {
            file = fileName;
            level = Level._INFO;
            DEBUG_writeToOutputWindow = false;
            loggingDisabled = false;

            if (mode == InitMode._OVERWRITE)
                clear();
        }

        public Log(string fileName, Level lvl)
        {
            file = fileName;
            level = lvl;
            DEBUG_writeToOutputWindow = false;
            loggingDisabled = (level == Level._NONE);
        }

        public Log(string fileName, Level lvl, InitMode mode)
        {
            file = fileName;
            level = lvl;
            DEBUG_writeToOutputWindow = false;
            loggingDisabled = (level == Level._NONE);

            if (!loggingDisabled)
            {
                if (mode == InitMode._OVERWRITE)
                    clear();
            }
        }

        private string levelLabel(Level lvl)
        {
            string label = "";

            switch (lvl)
            {
                case Level._INFO:
                    label = "<INFO>     ";
                    break;
                case Level._CRITICAL:
                    label = "<CRITICAL> ";
                    break;
                case Level._ERROR:
                    label = "<ERROR>    ";
                    break;
                case Level._WARNING:
                    label = "<WARNING>  ";
                    break;
                case Level._DEBUG:
                    label = "<DEBUG>    ";
                    break;
                case Level._MASSIVE:
                    label = "<MASSIVE>  ";
                    break;
            }

            return label;
        }

        private string timestamp()
        {
            string tstamp = DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss.fff] ");
            return tstamp;
        }

        private void writeMessage(string message)
        {
            if (DEBUG_writeToOutputWindow)
                Debug.WriteLine(message);

            if (File.Exists(file))
                stream = File.AppendText(file);
            else
                stream = File.CreateText(file);

            stream.WriteLine(message);
            stream.Close();
        }

        public void writeSeparator(string message)
        {
            if (loggingDisabled)
                return;

            if (File.Exists(file))
                stream = File.AppendText(file);
            else
                stream = File.CreateText(file);

            string time = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
            string bumper = "==================================================";
            stream.Write(stream.NewLine);
            stream.WriteLine(bumper);
            stream.WriteLine(message.Replace("[DATE]", time));
            stream.Write(stream.NewLine);
            stream.WriteLine(bumper);
            stream.Write(stream.NewLine);

            stream.Close();
        }

        public void critical(string text)
        {
            if (loggingDisabled)
                return;

            string message;
            if (level >= Level._CRITICAL)
            {
                message = timestamp() + levelLabel(Level._CRITICAL) + text;
                writeMessage(message);
            }
        }

        public void error(string text)
        {
            if (loggingDisabled)
                return;

            string message;

            if (level >= Level._ERROR)
            {
                message = timestamp() + levelLabel(Level._ERROR) + text;
                writeMessage(message);
            }
        }

        public void warning(string text)
        {
            if (loggingDisabled)
                return;

            string message;

            if (level >= Level._WARNING)
            {
                message = timestamp() + levelLabel(Level._WARNING) + text;
                writeMessage(message);
            }
        }

        public void info(string text)
        {
            if (loggingDisabled)
                return;

            string message;

            if (level >= Level._INFO)
            {
                message = timestamp() + levelLabel(Level._INFO) + text;
                writeMessage(message);
            }
        }

        public void debug(string text)
        {
            if (loggingDisabled)
                return;

            string message;

            if (level >= Level._DEBUG)
            {
                message = timestamp() + levelLabel(Level._DEBUG) + text;
                writeMessage(message);
            }
        }

        public void massive(string text)
        {
            if (loggingDisabled)
                return;

            string message;

            if (level >= Level._MASSIVE)
            {
                message = timestamp() + levelLabel(Level._MASSIVE) + text;
                writeMessage(message);
            }
        }

        public void echo(string text)
        {
            if (loggingDisabled)
                return;

            string message;

            message = INDENT + text;
            writeMessage(message);
        }

        public void echo(string text, bool indent)
        {
            if (loggingDisabled)
                return;

            string message;

            if (indent)
                message = INDENT + text;
            else
                message = text;

            writeMessage(message);
        }

        public void echo(string text, Level lvl)
        {
            if (loggingDisabled)
                return;

            if (level >= lvl)
            {
                string message = INDENT + text;
                writeMessage(message);
            }
        }

        public void echo(string text, Level lvl, bool indent)
        {
            if (loggingDisabled)
                return;

            string message;

            if (lvl >= level)
            {
                if (indent)
                    message = INDENT + text;
                else
                    message = text;

                writeMessage(message);
            }
        }

        public void linefeed()
        {
            if (loggingDisabled)
                writeMessage(Environment.NewLine);
        }

        public void linefeed(Level lvl)
        {
            if (loggingDisabled)
                return;

            if (level >= lvl)
                writeMessage(Environment.NewLine);
        }

        public void exception(Level lvl, Exception e)
        {
            if (loggingDisabled)
                return;

            if (lvl >= level)
            {
                writeMessage(timestamp() + levelLabel(lvl) + "An exception occurred:");
                string[] lines = e.ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                foreach (string line in lines)
                    echo(line, true);
            }
        }

        public void exception(Level lvl, Exception e, string text)
        {
            if (loggingDisabled)
                return;

            if (lvl >= level)
            {
                writeMessage(timestamp() + levelLabel(lvl) + text);
                string[] lines = e.ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                foreach (string line in lines)
                    echo(line, true);
            }
        }

        public void setLevel(Level lvl)
        {
            level = lvl;
            loggingDisabled = (level == Level._NONE);
        }

        public Level getLevel()
        {
            return level;
        }

        public string getLevelLabel()
        {
            string lvl = String.Empty;

            switch (level)
            {
                case Level._NONE:
                    lvl = "NONE";
                    break;
                case Level._CRITICAL:
                    lvl = "CRITICAL";
                    break;
                case Level._ERROR:
                    lvl = "ERROR";
                    break;
                case Level._WARNING:
                    lvl = "WARNING";
                    break;
                case Level._INFO:
                    lvl = "INFO";
                    break;
                case Level._DEBUG:
                    lvl = "DEBUG";
                    break;
                case Level._MASSIVE:
                    lvl = "MASSIVE";
                    break;
            }

            return lvl;
        }

        public void clear()
        {
            if (loggingDisabled)
                return;

            if (File.Exists(file))
                File.WriteAllText(file, String.Empty);
        }

        public void enableVSDebugLogging()
        {
            DEBUG_writeToOutputWindow = true;
        }

        public void disableVSDebugLogging()
        {
            DEBUG_writeToOutputWindow = false;
        }
    }
}
