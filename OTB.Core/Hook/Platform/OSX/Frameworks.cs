using System;
using System.Collections.Generic;
using System.Text;

namespace OTB.Core.Hook.Platform.OSX
{
    static class Frameworks
    {
        //https://developer.apple.com/library/content/documentation/MacOSX/Conceptual/OSX_Technology_Overview/SystemFrameworks/SystemFrameworks.html


        //NS
        //	Wrapper for including the Cocoa frameworks AppKit.framework, Foundation.framework, and CoreData.framework.
        public const string CocoaFramework = "/System/Library/Frameworks/Cocoa.framework/Cocoa";
        //CF
        public const string CoreFoundationFramework = "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";
        //AE, AX, ATSU, CG, CT, LS, PM, QD, UT
        public const string ApplicationServices = "/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices";



        //we can use Cocoa instead - it includes all this stuff under one
        //private const string FoundationFramework = "/System/Library/Frameworks/Foundation.framework/Foundation";
        //private const string AppKitFramework = "/System/Library/Frameworks/AppKit.framework/AppKit";



    }
}
