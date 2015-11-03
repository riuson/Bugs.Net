﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Updater.Tarx
{
    public class Unpacker : IDisposable
    {
        private Stream mStreamIn;
        private Log mLog;
        private IUnpacker mUnpackerPrivate;

        public XDocument XHeader { get; private set; }

        public Unpacker(Stream streamIn, Log log, Action<XElement> headerCallback)
        {
            this.mStreamIn = streamIn;

            if (log != null)
            {
                this.mLog = log;
            }
            else
            {
                this.mLog = (message) =>
                {
                };
            }

            this.XHeader = this.GetHeader();

            if (headerCallback != null)
            {
                headerCallback(this.XHeader.Root.Element("header"));
            }

            string mode = this.XHeader.Root.Element("header").Element("mode").Value;

            switch (mode)
            {
                case "linear":
                    {
                        this.mUnpackerPrivate = new UnpackerLinear(this.mStreamIn, this.XHeader, log);
                        break;
                    }
                case "postponed":
                    {
                        this.mUnpackerPrivate = new UnpackerPostponed(this.mStreamIn, this.XHeader, log);
                        break;
                    }
            }
        }

        public void Dispose()
        {
            this.mUnpackerPrivate.Dispose();
        }

        private XDocument GetHeader()
        {
            XDocument xHeader = PackerHelper.ReadNextDocument(this.mStreamIn);
            return xHeader;
        }

        public bool CollectContentInfo()
        {
            return this.mUnpackerPrivate.CollectContentInfo();
        }

        public XDocument XContent
        {
            get
            {
                return this.mUnpackerPrivate.XContent;
            }
        }

        public Action<XElement> HeaderAfter
        {
            get
            {
                return this.mUnpackerPrivate.HeaderAfter;
            }
            set
            {
                this.mUnpackerPrivate.HeaderAfter = value;
            }
        }

        public bool UnpackTo(DirectoryInfo directory, Func<XElement, Boolean> xitemCallback)
        {
            if (xitemCallback == null)
            {
                xitemCallback = (item) =>
                {
                    return true;
                };
            }

            return this.mUnpackerPrivate.UnpackTo(directory, xitemCallback);
        }
    }
}
