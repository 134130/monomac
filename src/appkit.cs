//
// Copyright 2011-2012 Xamarin, Inc.
// Copyright 2010, 2011, Novell, Inc.
// Copyright 2010, Kenneth Pouncey
// Coprightt 2010, James Clancey
// Copyright 2011, Curtis Wensley
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//
// appkit.cs: Definitions for AppKit
//

// TODO: turn NSAnimatablePropertyCOntainer into a system similar to UIAppearance

using System;
using System.Diagnostics;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using MonoMac.CoreGraphics;
using MonoMac.CoreImage;
using MonoMac.CoreAnimation;
using MonoMac.CoreData;
using MonoMac.OpenGL;

#if MAC64
using nint = System.Int64;
using nuint = System.UInt64;
using nfloat = System.Double;
#else
using nint = System.Int32;
using nuint = System.UInt32;
using nfloat = System.Single;
#if SDCOMPAT
using CGPoint = System.Drawing.PointF;
using CGSize = System.Drawing.SizeF;
using CGRect = System.Drawing.RectangleF;
#endif
#endif

namespace MonoMac.AppKit {
	[AttributeUsage( AttributeTargets.All, AllowMultiple=true )]
	sealed class VerifyAttribute : System.Attribute
	{
		public VerifyAttribute(string description)
		{
			Description = description;
		}
		public string Description { get; private set; }
		public int Line { get; set; }
	}

	//[BaseType (typeof (NSObject))]
	//interface CIImage {
	//	[Export ("drawInRect:fromRect:operation:fraction:")]
	//	void Draw (CGRect inRect, CGRect fromRect, NSCompositingOperation operation, nfloat fractionDelta);
	//
	//	[Export ("drawAtPoint:fromRect:operation:fraction:")]
	//	void DrawAtPoint (CGPoint atPoint, CGRect fromRect, NSCompositingOperation operation, nfloat fractionDelta);
	//}
	
	[BaseType (typeof (NSCell))]
	public interface NSActionCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);
	
		[Export ("target"), NullAllowed]
		NSObject Target  { get; set; }
	
		[Export ("action"), NullAllowed]
		Selector Action  { get; set; }
	
		[Export ("tag")]
		nint Tag  { get; set; }
	
	}

	//
	// Inlined, not really an object implementation
	//
	public interface NSAnimatablePropertyContainer {
		[Export ("animator")]
		NSObject Animator { [return: Proxy] get; }
	
		[Export ("animations")]
		NSDictionary Animations { get; set; }
	
		[Export ("animationForKey:")]
		NSObject AnimationFor (NSString key);
	
		[Static, Export ("defaultAnimationForKey:")]
		NSObject DefaultAnimationFor (NSString key);
	}
	
	public interface NSAnimationProgressMarkEventArgs {
		[Export ("NSAnimationProgressMark")]
		nfloat Progress { get; }
	}

	[BaseType (typeof (NSObject), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSAnimationDelegate)})]
	public interface NSAnimation {
		[Export ("initWithDuration:animationCurve:")]
		IntPtr Constant (double duration, NSAnimationCurve animationCurve);
	
		[Export ("startAnimation")]
		void StartAnimation ();
	
		[Export ("stopAnimation")]
		void StopAnimation ();
	
		[Export ("isAnimating")]
		bool IsAnimating ();
	
		[Export ("currentProgress")]
		nfloat CurrentProgress { get; set; }
	
		[Export ("duration")]
		double Duration  { get; set; }
	
		[Export ("animationBlockingMode")]
		NSAnimationBlockingMode AnimationBlockingMode  { get; set; }
	
		[Export ("frameRate")]
		nfloat FrameRate  { get; set; }
	
		[Export ("animationCurve")]
		NSAnimationCurve AnimationCurve  { get; set; }
	
		[Export ("currentValue")]
		nfloat CurrentValue { get; }
	
		[Export ("delegate")]
		NSAnimationDelegate Delegate  { get; set; }
	
		[Export ("progressMarks")]
		NSNumber [] ProgressMarks  { get; set; }
	
		[Export ("addProgressMark:")]
		void AddProgressMark (nfloat progressMark);
	
		[Export ("removeProgressMark:")]
		void RemoveProgressMark (nfloat progressMark);
	
		[Export ("startWhenAnimation:reachesProgress:")]
		void StartWhenAnimationReaches (NSAnimation animation, nfloat startProgress);
	
		[Export ("stopWhenAnimation:reachesProgress:")]
		void StopWhenAnimationReaches (NSAnimation animation, nfloat stopProgress);
	
		[Export ("clearStartAnimation")]
		void ClearStartAnimation ();
	
		[Export ("clearStopAnimation")]
		void ClearStopAnimation ();

		// TODO
		//[Export ("runLoopModesForAnimating")]
		//NSArray* runLoopModesForAnimating ();

		[Notification (typeof (NSAnimationProgressMarkEventArgs)), Field ("NSAnimationProgressMarkNotification")]
		NSString ProgressMarkNotification { get; }

		[Field ("NSAnimationProgressMark")]
		NSString ProgressMark { get; }

		[Field ("NSAnimationTriggerOrderIn")]
		NSString TriggerOrderIn { get; }

		[Field ("NSAnimationTriggerOrderOut")]
		NSString TriggerOrderOut { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSAnimationDelegate {
		[Export ("animationShouldStart:"), DelegateName ("NSAnimationPredicate"), DefaultValue (true)]
		bool AnimationShouldStart (NSAnimation animation);
	
		[Export ("animationDidStop:"), EventArgs ("NSAnimation")]
		void AnimationDidStop (NSAnimation animation);
	
		[Export ("animationDidEnd:"), EventArgs ("NSAnimation")]
		void AnimationDidEnd (NSAnimation animation);
	
		[Export ("animation:valueForProgress:"), DelegateName ("NSAnimationProgress"), DefaultValueFromArgumentAttribute ("progress")]
		nfloat ComputeAnimationCurve (NSAnimation animation, nfloat progress);
	
		[Export ("animation:didReachProgressMark:"), EventArgs ("NSAnimation")]
		void AnimationDidReachProgressMark (NSAnimation animation, nfloat progress);
	}

	[BaseType (typeof (NSObject))]
	public partial interface NSAnimationContext {
		[Static]
		[Export ("beginGrouping")]
		void BeginGrouping ();

		[Static]
		[Export ("endGrouping")]
		void EndGrouping ();

		[Static]
		[Export ("currentContext")]
		NSAnimationContext CurrentContext { get; }

		//Detected properties
		[Export ("duration")]
		double Duration { get; set; }

		[Lion, Export ("completionHandler")]
		Action CompletionHandler { get; set; }

		[Static]
		[Lion, Export ("runAnimationGroup:completionHandler:")]
		void RunAnimation (Action<NSAnimationContext> changes, Action completionHandler);
    
		[Lion, Export ("timingFunction")]
		CAMediaTimingFunction TimingFunction { get; set; }

		[MountainLion, Export ("allowsImplicitAnimation")]
		bool AllowsImplicitAnimation { get; set; }
	}
	
	[BaseType (typeof (NSObject), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSAlertDelegate)})]
	public interface NSAlert {
		[Static, Export ("alertWithError:")]
		NSAlert WithError (NSError  error);
	
		[Static, Export ("alertWithMessageText:defaultButton:alternateButton:otherButton:informativeTextWithFormat:")]
		NSAlert WithMessage([NullAllowed] string message, [NullAllowed] string defaultButton, [NullAllowed] string alternateButton, [NullAllowed]  string otherButton, string full);
	
		[Export ("messageText")]
		string MessageText { get; set; }
	
		[Export ("informativeText")]
		string InformativeText { get; set; }
	
		[Export ("icon")]
		NSImage Icon { get; set; }
	
		[Export ("addButtonWithTitle:")]
		NSButton AddButton (string title);
	
		[Export ("buttons")]
		NSButton [] Buttons { get; }
	
		[Export ("showsHelp")]
		bool ShowsHelp { get; set; }
	
		[Export ("helpAnchor")]
		string HelpAnchor { get; set; }
	
		[Export ("alertStyle")]
		NSAlertStyle AlertStyle { get; set; }
	
		[Export ("delegate")]
		NSAlertDelegate Delegate { get; set; } 
	
		[Export ("showsSuppressionButton")]
		bool ShowsSuppressionButton { get; set; } 
	
		[Export ("suppressionButton")]
		NSButton SuppressionButton { get; } 
	
		[Export ("accessoryView"), NullAllowed]
		NSView AccessoryView { get; set; } 
	
		[Export ("layout")]
		void Layout ();
	
		[Export ("runModal")]
		nint RunModal ();
	
		[Export ("beginSheetModalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		void BeginSheet ([NullAllowed] NSWindow  window, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);
	
		[Export ("window")]
		NSPanel Window  { get; }
	
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSAlertDelegate {
		[Export ("alertShowHelp:"), DelegateName ("NSAlertPredicate"), DefaultValue (false)]
		bool ShowHelp (NSAlert  alert);
	}

	public interface NSApplicationDidFinishLaunchingEventArgs {
		[Export ("NSApplicationLaunchIsDefaultLaunchKey")]
		bool IsLaunchDefault { get; }

		[ProbePresence, Export ("NSApplicationLaunchUserNotificationKey")]
		bool IsLaunchFromUserNotification { get; }
	}
	
	[Mac (10,9)]
	// [NoMacCatalyst]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	public interface NSAppearanceCustomization {

		[Mac (10,9)]
		[NullAllowed]
		[Export ("appearance", ArgumentSemantic.Strong)]
		NSAppearance Appearance { get; set; }

		[Mac (10,9)]
		[Export ("effectiveAppearance", ArgumentSemantic.Strong)]
		NSAppearance EffectiveAppearance { get; }
	}
	

	[BaseType (typeof (NSResponder), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSApplicationDelegate) })]
	[DisableDefaultCtor] // An uncaught exception was raised: Creating more than one Application
	public interface NSApplication : NSWindowRestoration, NSAppearanceCustomization {
		[Export ("sharedApplication"), Static, ThreadSafe]
		NSApplication SharedApplication { get; }
	
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSApplicationDelegate Delegate { get; set; }
	
		[Export ("context")]
		NSGraphicsContext Context { get; }
	
		[Export ("hide:")]
		void Hide (NSObject sender);
	
		[Export ("unhide:")]
		void Unhide (NSObject sender);
	
		[Export ("unhideWithoutActivation")]
		void UnhideWithoutActivation ();
	
		[Export ("windowWithWindowNumber:")]
		NSWindow WindowWithWindowNumber (nint windowNum);
	
		[Export ("mainWindow")]
		NSWindow MainWindow { get; }
	
		[Export ("keyWindow")]
		NSWindow KeyWindow { get; }
	
		[Export ("isActive")]
		bool Active { get; }
	
		[Export ("isHidden")]
		bool Hidden { get; }
	
		[Export ("isRunning")]
		bool Running { get; }
	
		[Export ("deactivate")]
		void Deactivate ();
	
		[Export ("activateIgnoringOtherApps:")]
		void ActivateIgnoringOtherApps (bool flag);
	
		[Export ("hideOtherApplications:")]
		void HideOtherApplications (NSObject sender);
	
		[Export ("unhideAllApplications:")]
		void UnhideAllApplications (NSObject sender);
	
		[Export ("finishLaunching")]
		void FinishLaunching ();
	
		[Export ("run")]
		void Run ();
	
		[Export ("runModalForWindow:")]
		nint RunModalForWindow (NSWindow theWindow);
	
		[Export ("stop:")]
		void Stop (NSObject sender);
	
		[Export ("stopModal")]
		void StopModal ();
	
		[Export ("stopModalWithCode:")]
		void StopModalWithCode (nint returnCode);
	
		[Export ("abortModal"), ThreadSafe]
		void AbortModal ();
	
		[Export ("modalWindow")]
		NSWindow ModalWindow { get; }
	
		[Export ("beginModalSessionForWindow:")]
		IntPtr BeginModalSession (NSWindow theWindow);
	
		[Export ("runModalSession:")]
		nint RunModalSession (IntPtr session);
	
		[Export ("endModalSession:")]
		void EndModalSession (IntPtr session);
	
		[Export ("terminate:")]
		void Terminate (NSObject sender);
	
		[Export ("requestUserAttention:")]
		nint RequestUserAttention (NSRequestUserAttentionType requestType);
	
		[Export ("cancelUserAttentionRequest:")]
		void CancelUserAttentionRequest (nint request);
	
		[Export ("beginSheet:modalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		void BeginSheet (NSWindow sheet, NSWindow docWindow, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);
	
		[Export ("endSheet:")]
		void EndSheet (NSWindow sheet);
	
		[Export ("endSheet:returnCode:")]
		void EndSheet (NSWindow  sheet, nint returnCode);
	
		[Export ("nextEventMatchingMask:untilDate:inMode:dequeue:"), Protected]
		NSEvent NextEvent (NSEventMask mask, [NullAllowed] NSDate expiration, NSString runLoopMode, bool deqFlag);

		[Obsolete ("Use the 'NextEvent (NSEventMask, NSDate, [NSRunLoopMode|NSString], bool)' overloads instead.")]
		[Wrap ("NextEvent ((NSEventMask) (ulong) mask, expiration, (NSString) mode, deqFlag)", IsVirtual = true), Protected]
		NSEvent NextEvent (nuint mask, NSDate expiration, string mode, bool deqFlag);

		// NSEventMask must be casted to nuint to preserve the NSEventMask.Any special value on 64 bit systems. NSEventMask is not [Native].
		[Obsolete ("Use the 'NextEvent (NSEventMask, NSDate, [NSRunLoopMode|NSString], bool)' overloads instead.")]
		[Wrap ("NextEvent (mask, expiration, (NSString) mode, deqFlag)")]
		NSEvent NextEvent (NSEventMask mask, NSDate expiration, string mode, bool deqFlag);
	
		// NSEventMask must be casted to nuint to preserve the NSEventMask.Any special value on 64 bit systems. NSEventMask is not [Native].
		[Wrap ("NextEvent (mask, expiration, NSRunLoop.FromEnum(runLoopMode), deqFlag)")]
		NSEvent NextEvent (NSEventMask mask, NSDate expiration, NSRunLoopMode runLoopMode, bool deqFlag);
	
		[Export ("discardEventsMatchingMask:beforeEvent:"), Protected]
		void DiscardEvents (nuint mask, NSEvent lastEvent);
	
                [ThreadSafe]
		[Export ("postEvent:atStart:")]
		void PostEvent (NSEvent theEvent, bool atStart);
	
		[Export ("currentEvent")]
		NSEvent CurrentEvent { get; }
	
		[Export ("sendEvent:")]
		void SendEvent (NSEvent theEvent);
	
		[Export ("preventWindowOrdering")]
		void PreventWindowOrdering ();
	
		[Export ("makeWindowsPerform:inOrder:")]
		NSWindow MakeWindowsPerform (Selector aSelector, bool inOrder);
	
		[Export ("windows")]
		NSWindow [] Windows { get; }
	
		[Export ("setWindowsNeedUpdate:")]
		void SetWindowsNeedUpdate (bool needUpdate);
	
		[Export ("updateWindows")]
		void UpdateWindows ();
	
		[Export ("setMainMenu:")]
		[Obsolete ("Use MainMenu property")]
		void SetMainMenu (NSMenu  aMenu);
	
		[Export ("mainMenu")]
		NSMenu MainMenu { get; set; }
	
		[Export ("helpMenu")]
		NSMenu HelpMenu { get; [NullAllowed] set; }
	
		[Export ("applicationIconImage")]
		NSImage ApplicationIconImage { get; set; }
	
		[Export ("activationPolicy"), Protected]
		NSApplicationActivationPolicy GetActivationPolicy ();

		[Export ("setActivationPolicy:"), Protected]
		bool SetActivationPolicy (NSApplicationActivationPolicy activationPolicy);

		[Export ("dockTile")]
		NSDockTile DockTile { get; }
	
		[Export ("sendAction:to:from:")]
		bool SendAction (Selector theAction, NSObject theTarget, NSObject sender);
	
		[Export ("targetForAction:")]
		NSObject TargetForAction (Selector theAction);
	
		[Export ("targetForAction:to:from:")]
		NSObject TargetForAction (Selector theAction, NSObject theTarget, NSObject sender);
	
		[Export ("tryToPerform:with:")]
		bool TryToPerform (Selector anAction, NSObject target);
	
		[Export ("validRequestorForSendType:returnType:")]
		NSObject ValidRequestor (string sendType, string returnType);
	
		[Export ("reportException:")]
		void ReportException (NSException theException);
	
		[Static]
		[Export ("detachDrawingThread:toTarget:withObject:")]
		void DetachDrawingThread (Selector selector, NSObject target, NSObject argument);
	
		[Export ("replyToApplicationShouldTerminate:")]
		void ReplyToApplicationShouldTerminate (bool shouldTerminate);
	
		[Export ("replyToOpenOrPrint:")]
		void ReplyToOpenOrPrint (NSApplicationDelegateReply reply);
	
		[Export ("orderFrontCharacterPalette:")]
		void OrderFrontCharacterPalette (NSObject sender);
	
		[Export ("presentationOptions")]
		NSApplicationPresentationOptions PresentationOptions { get; set; }
	
		[Export ("currentSystemPresentationOptions")]
		NSApplicationPresentationOptions CurrentSystemPresentationOptions { get; }
	
		[Export ("windowsMenu")]
		NSMenu WindowsMenu { get; set; }
	
		[Export ("arrangeInFront:")]
		void ArrangeInFront (NSObject sender);
	
		[Export ("removeWindowsItem:")]
		void RemoveWindowsItem (NSWindow  win);
	
		[Export ("addWindowsItem:title:filename:")]
		void AddWindowsItem (NSWindow  win, string title, bool isFilename);
	
		[Export ("changeWindowsItem:title:filename:")]
		void ChangeWindowsItem (NSWindow  win, string title, bool isFilename);
	
		[Export ("updateWindowsItem:")]
		void UpdateWindowsItem (NSWindow  win);
	
		[Export ("miniaturizeAll:")]
		void MiniaturizeAll (NSObject sender);
	
		[Export ("isFullKeyboardAccessEnabled")]
		bool FullKeyboardAccessEnabled { get; }

		[Export ("servicesProvider")]
		NSObject ServicesProvider { get; set; }
	
		[Export ("userInterfaceLayoutDirection")]
		NSApplicationLayoutDirection UserInterfaceLayoutDirection { get; }

		[Export ("servicesMenu")]
		NSMenu ServicesMenu { get; set; }

		// From NSColorPanel
		[Export ("orderFrontColorPanel:")]
		void OrderFrontColorPanel (NSObject sender);

		[Lion, Export ("disableRelaunchOnLogin"), ThreadSafe]
		void DisableRelaunchOnLogin ();

		[Lion, Export ("enableRelaunchOnLogin"), ThreadSafe]
		void EnableRelaunchOnLogin ();

		[Lion, Export ("enabledRemoteNotificationTypes")]
		NSRemoteNotificationType EnabledRemoteNotificationTypes ();

		[Lion, Export ("registerForRemoteNotificationTypes:")]
		void RegisterForRemoteNotificationTypes (NSRemoteNotificationType types);

		[Lion, Export ("unregisterForRemoteNotifications")]
		void UnregisterForRemoteNotifications ();

		[Notification, Field ("NSApplicationDidBecomeActiveNotification")]
		NSString DidBecomeActiveNotification { get; }

		[Notification, Field ("NSApplicationDidHideNotification")]
		NSString DidHideNotification { get; }

		[Notification (typeof (NSApplicationDidFinishLaunchingEventArgs)), Field ("NSApplicationDidFinishLaunchingNotification")]
		NSString DidFinishLaunchingNotification { get; }

		[Notification, Field ("NSApplicationDidResignActiveNotification")]
		NSString DidResignActiveNotification { get; }

		[Notification, Field ("NSApplicationDidUnhideNotification")]
		NSString DidUnhideNotification { get; }

		[Notification, Field ("NSApplicationDidUpdateNotification")]
		NSString DidUpdateNotification { get; }

		[Notification, Field ("NSApplicationWillBecomeActiveNotification")]
		NSString WillBecomeActiveNotification { get; }

		[Notification, Field ("NSApplicationWillHideNotification")]
		NSString WillHideNotification { get; }

		[Notification, Field ("NSApplicationWillFinishLaunchingNotification")]
		NSString WillFinishLaunchingNotification { get; }

		[Notification, Field ("NSApplicationWillResignActiveNotification")]
		NSString WillResignActiveNotification { get; }

		[Notification, Field ("NSApplicationWillUnhideNotification")]
		NSString WillUnhideNotification { get; }

		[Notification, Field ("NSApplicationWillUpdateNotification")]
		NSString WillUpdateNotification { get; }

		[Notification, Field ("NSApplicationWillTerminateNotification")]
		NSString WillTerminateNotification { get; }

		[Notification, Field ("NSApplicationDidChangeScreenParametersNotification")]
		NSString DidChangeScreenParametersNotification { get; }

		[Lion, Field ("NSApplicationLaunchIsDefaultLaunchKey")]
		NSString LaunchIsDefaultLaunchKey  { get; }

		[Lion, Field ("NSApplicationLaunchRemoteNotificationKey")]
		NSString LaunchRemoteNotificationKey { get; }

		[Notification, Field ("NSApplicationDidFinishRestoringWindowsNotification")]
		NSString DidFinishRestoringWindowsNotification { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSApplicationDelegate {
		[Export ("applicationShouldTerminate:"), DelegateName ("NSApplicationTermination"), DefaultValue (NSApplicationTerminateReply.Now)]
		NSApplicationTerminateReply ApplicationShouldTerminate (NSApplication  sender);
	
		[Export ("application:openFile:"), DelegateName ("NSApplicationFile"), DefaultValue (false)]
		bool OpenFile (NSApplication sender, string  filename);
	
		[Export ("application:openFiles:"), EventArgs ("NSApplicationFiles")]
		void OpenFiles (NSApplication sender, string [] filenames);
	
		[Export ("application:openTempFile:"), DelegateName ("NSApplicationFile"), DefaultValue (false)]
		bool OpenTempFile (NSApplication sender, string  filename);
	
		[Export ("applicationShouldOpenUntitledFile:"), DelegateName ("NSApplicationPredicate"), DefaultValue (false)]
		bool ApplicationShouldOpenUntitledFile (NSApplication  sender);
	
		[Export ("applicationOpenUntitledFile:"), DelegateName ("NSApplicationPredicate"), DefaultValue (false)]
		bool ApplicationOpenUntitledFile (NSApplication sender);
	
		[Export ("application:openFileWithoutUI:"), DelegateName ("NSApplicationFileCommand"), DefaultValue (false)]
		bool OpenFileWithoutUI (NSObject sender, string filename);
	
		[Export ("application:printFile:"), DelegateName ("NSApplicationFile"), DefaultValue (false)]
		bool PrintFile (NSApplication sender, string filename);
	
		[Export ("application:printFiles:withSettings:showPrintPanels:"), DelegateName ("NSApplicationPrint"), DefaultValue (NSApplicationPrintReply.Failure)]
		NSApplicationPrintReply PrintFiles (NSApplication application, string [] fileNames, NSDictionary printSettings, bool showPrintPanels);
	
		[Export ("applicationShouldTerminateAfterLastWindowClosed:"), DelegateName ("NSApplicationPredicate"), DefaultValue (false)]
		bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender);
	
		[Export ("applicationShouldHandleReopen:hasVisibleWindows:"), DelegateName ("NSApplicationReopen"), DefaultValue (false)]
		bool ApplicationShouldHandleReopen (NSApplication sender, bool hasVisibleWindows);
	
		[Export ("applicationDockMenu:"), DelegateName ("NSApplicationMenu"), DefaultValue (null)]
		NSMenu ApplicationDockMenu (NSApplication sender);
	
		[Export ("application:willPresentError:"), DelegateName ("NSApplicationError"), DefaultValue (null)]
		NSError WillPresentError (NSApplication application, NSError error);
	
		[Export ("applicationWillFinishLaunching:"), EventArgs ("NSNotification")]
		void WillFinishLaunching (NSNotification notification);
	
		[Export ("applicationDidFinishLaunching:"), EventArgs ("NSNotification")]
		void DidFinishLaunching (NSNotification notification);
	
		[Export ("applicationWillHide:"), EventArgs ("NSNotification")]
		void WillHide (NSNotification notification);
	
		[Export ("applicationDidHide:"), EventArgs ("NSNotification")]
		void DidHide (NSNotification notification);
	
		[Export ("applicationWillUnhide:"), EventArgs ("NSNotification")]
		void WillUnhide (NSNotification notification);
	
		[Export ("applicationDidUnhide:"), EventArgs ("NSNotification")]
		void DidUnhide (NSNotification notification);
	
		[Export ("applicationWillBecomeActive:"), EventArgs ("NSNotification")]
		void WillBecomeActive (NSNotification notification);
	
		[Export ("applicationDidBecomeActive:"), EventArgs ("NSNotification")]
		void DidBecomeActive (NSNotification notification);
	
		[Export ("applicationWillResignActive:"), EventArgs ("NSNotification")]
		void WillResignActive (NSNotification notification);
	
		[Export ("applicationDidResignActive:"), EventArgs ("NSNotification")]
		void DidResignActive (NSNotification notification);
	
		[Export ("applicationWillUpdate:"), EventArgs ("NSNotification")]
		void WillUpdate (NSNotification notification);
	
		[Export ("applicationDidUpdate:"), EventArgs ("NSNotification")]
		void DidUpdate (NSNotification notification);
	
		[Export ("applicationWillTerminate:"), EventArgs ("NSNotification")]
		void WillTerminate (NSNotification notification);
	
		[Export ("applicationDidChangeScreenParameters:"), EventArgs ("NSNotification")]
		void ScreenParametersChanged (NSNotification notification);

		[Export ("registerServicesMenuSendTypes:returnTypes:"), EventArgs ("NSApplicationRegister")]
		void RegisterServicesMenu (string [] sendTypes, string [] returnTypes);
	
		[Export ("writeSelectionToPasteboard:types:"), DelegateName ("NSApplicationSelection"), DefaultValue (false)]
		bool WriteSelectionToPasteboard (NSPasteboard board, string [] types);
	
		[Export ("readSelectionFromPasteboard:"), DelegateName ("NSPasteboardPredicate"), DefaultValue (false)]
		bool ReadSelectionFromPasteboard (NSPasteboard pboard);
	
		[Export ("orderFrontStandardAboutPanel:"), EventArgs ("NSObject")]
		void OrderFrontStandardAboutPanel (NSObject sender);
	
		[Export ("orderFrontStandardAboutPanelWithOptions:"), EventArgs ("NSDictionary")]
		void OrderFrontStandardAboutPanelWithOptions (NSDictionary optionsDictionary);

		[Lion, Export ("application:didRegisterForRemoteNotificationsWithDeviceToken:"), EventArgs ("NSData")]
		void RegisteredForRemoteNotifications (NSApplication application, NSData deviceToken);

		[Lion, Export ("application:didFailToRegisterForRemoteNotificationsWithError:"), EventArgs ("NSError", true)]
		void FailedToRegisterForRemoteNotifications (NSApplication application, NSError error);

		[Lion, Export ("application:didReceiveRemoteNotification:"), EventArgs ("NSDictionary")]
		void ReceivedRemoteNotification (NSApplication application, NSDictionary userInfo);

		[Lion, Export ("application:willEncodeRestorableState:"), EventArgs ("NSCoder")]
		void WillEncodeRestorableState (NSApplication app, NSCoder encoder);

		[Lion, Export ("application:didDecodeRestorableState:"), EventArgs ("NSCoder")]
		void DecodedRestorableState (NSApplication app, NSCoder state);
	}
	
	[BaseType (typeof (NSObjectController))]
	public interface NSArrayController {
		[Export ("rearrangeObjects")]
		void RearrangeObjects ();

		[Export ("automaticRearrangementKeyPaths")]
		NSObject [] AutomaticRearrangementKeyPaths ();

		[Export ("didChangeArrangementCriteria")]
		void DidChangeArrangementCriteria ();

		[Export ("arrangeObjects:")]
		NSObject [] ArrangeObjects (NSObject [] objects);

		[Export ("arrangedObjects")]
		NSObject [] ArrangedObjects ();

		[Export ("addSelectionIndexes:")]
		bool AddSelectionIndexes (NSIndexSet indexes);

		[Export ("removeSelectionIndexes:")]
		bool RemoveSelectionIndexes (NSIndexSet indexes);

		[Export ("addSelectedObjects:")]
		bool AddSelectedObjects (NSObject [] objects);

		[Export ("removeSelectedObjects:")]
		bool RemoveSelectedObjects (NSObject [] objects);

		[Export ("add:")]
		void Add (NSObject sender);

		[Export ("remove:")]
		void RemoveOp (NSObject sender);

		[Export ("insert:")]
		void Insert (NSObject sender);

		[Export ("canInsert")]
		bool CanInsert ();

		[Export ("selectNext:")]
		void SelectNext (NSObject sender);

		[Export ("selectPrevious:")]
		void SelectPrevious (NSObject sender);

		[Export ("canSelectNext")]
		bool CanSelectNext ();

		[Export ("canSelectPrevious")]
		bool CanSelectPrevious ();

		[Export ("addObject:")]
		void AddObject (NSObject aObject);

		[Export ("addObjects:")]
		void AddObjects (NSArray objects);

		[Export ("insertObject:atArrangedObjectIndex:")]
		void Insert (NSObject aObject, nint index);

		[Export ("insertObjects:atArrangedObjectIndexes:")]
		void Insert (NSObject [] objects, NSIndexSet indexes);

		[Export ("removeObjectAtArrangedObjectIndex:")]
		void RemoveAt (nint index);

		[Export ("removeObjectsAtArrangedObjectIndexes:")]
		void Remove (NSIndexSet indexes);

		[Export ("removeObject:")]
		void Remove (NSObject aObject);

		[Export ("removeObjects:")]
		void Remove (NSObject [] objects);

		//Detected properties
		[Export ("automaticallyRearrangesObjects")]
		bool AutomaticallyRearrangesObjects { get; set; }

		[Export ("sortDescriptors")]
		NSObject [] SortDescriptors { get; set; }

		[Export ("filterPredicate")]
		NSPredicate FilterPredicate { get; set; }

		[Export ("clearsFilterPredicateOnInsertion")]
		bool ClearsFilterPredicateOnInsertion { get; set; }

		[Export ("avoidsEmptySelection")]
		bool AvoidsEmptySelection { get; set; }

		[Export ("preservesSelection")]
		bool PreservesSelection { get; set; }

		[Export ("selectsInsertedObjects")]
		bool SelectsInsertedObjects { get; set; }

		[Export ("alwaysUsesMultipleValuesMarker")]
		bool AlwaysUsesMultipleValuesMarker { get; set; }

		[Export ("selectionIndexes"), Protected]
		NSIndexSet GetSelectionIndexes ();

		[Export ("setSelectionIndexes:"), Protected]
		bool SetSelectionIndexes (NSIndexSet indexes);

		[Export ("selectionIndex"), Protected]
		nuint GetSelectionIndex ();

		[Export ("setSelectionIndex:"), Protected]
		bool SetSelectionIndex (nuint index);

		[Export ("selectedObjects"), Protected]
		NSObject [] GetSelectedObjects ();

		[Export ("setSelectedObjects:"), Protected]
		bool SetSelectedObjects (NSObject [] objects);
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSBezierPath {

		[Static]
		[Export ("bezierPathWithRect:")]
		NSBezierPath FromRect (CGRect rect);

		[Static]
		[Export ("bezierPathWithOvalInRect:")]
		NSBezierPath FromOvalInRect (CGRect rect);

		[Static]
		[Export ("bezierPathWithRoundedRect:xRadius:yRadius:")]
		NSBezierPath FromRoundedRect (CGRect rect, nfloat xRadius, nfloat yRadius);

		[Static]
		[Export ("fillRect:")]
		void FillRect (CGRect rect);

		[Static]
		[Export ("strokeRect:")]
		void StrokeRect (CGRect rect);

		[Static]
		[Export ("clipRect:")]
		void ClipRect (CGRect rect);

		[Static]
		[Export ("strokeLineFromPoint:toPoint:")]
		void StrokeLine (CGPoint point1, CGPoint point2);

		//IntPtr is exposed because the packedGlyphs should be treated as a "black box"
		[Static]
		[Export ("drawPackedGlyphs:atPoint:")]
		void DrawPackedGlyphsAtPoint (IntPtr packedGlyphs, CGPoint point);

		[Export ("moveToPoint:")]
		void MoveTo (CGPoint point);

		[Export ("lineToPoint:")]
		void LineTo (CGPoint point);

		[Export ("curveToPoint:controlPoint1:controlPoint2:")]
		void CurveTo (CGPoint endPoint, CGPoint controlPoint1, CGPoint controlPoint2);

		[Export ("closePath")]
		void ClosePath ();

		[Export ("removeAllPoints")]
		void RemoveAllPoints ();

		[Export ("relativeMoveToPoint:")]
		void RelativeMoveTo (CGPoint point);

		[Export ("relativeLineToPoint:")]
		void RelativeLineTo (CGPoint point);

		[Export ("relativeCurveToPoint:controlPoint1:controlPoint2:")]
		void RelativeCurveTo (CGPoint endPoint, CGPoint controlPoint1, CGPoint controlPoint2);

		[Export ("getLineDash:count:phase:"), Internal]
		void _GetLineDash (IntPtr pattern, out nint count, out nfloat phase);

		[Export ("setLineDash:count:phase:"), Internal]
		void _SetLineDash (IntPtr pattern, nint count, nfloat phase);

		[Export ("stroke")]
		void Stroke ();

		[Export ("fill")]
		void Fill ();

		[Export ("addClip")]
		void AddClip ();

		[Export ("setClip")]
		void SetClip ();

		[Export ("bezierPathByFlatteningPath")]
		NSBezierPath BezierPathByFlatteningPath ();

		[Export ("bezierPathByReversingPath")]
		NSBezierPath BezierPathByReversingPath ();

		[Export ("transformUsingAffineTransform:")]
		void TransformUsingAffineTransform (NSAffineTransform transform);

		[Export ("isEmpty")]
		bool IsEmpty { get; }

		[Export ("currentPoint")]
		CGPoint CurrentPoint { get; }

		[Export ("controlPointBounds")]
		CGRect ControlPointBounds { get; }

		[Export ("bounds")]
		CGRect Bounds { get; }

		[Export ("elementCount")]
		nint ElementCount { get; }

		[Export ("elementAtIndex:associatedPoints:"), Internal]
		NSBezierPathElement _ElementAt (nint index, IntPtr points);

		[Export ("elementAtIndex:")]
		NSBezierPathElement ElementAt (nint index);

		[Export ("setAssociatedPoints:atIndex:"), Internal]
		void _SetAssociatedPointsAtIndex (IntPtr points, nint index);

		[Export ("appendBezierPath:")]
		void AppendPath (NSBezierPath path);

		[Export ("appendBezierPathWithRect:")]
		void AppendPathWithRect (CGRect rect);

		[Export ("appendBezierPathWithPoints:count:"), Internal]
		void _AppendPathWithPoints (IntPtr points, nint count);

		[Export ("appendBezierPathWithOvalInRect:")]
		void AppendPathWithOvalInRect (CGRect rect);

		[Export ("appendBezierPathWithArcWithCenter:radius:startAngle:endAngle:clockwise:")]
		void AppendPathWithArc (CGPoint center, nfloat radius, nfloat startAngle, nfloat endAngle, bool clockwise);

		[Export ("appendBezierPathWithArcWithCenter:radius:startAngle:endAngle:")]
		void AppendPathWithArc (CGPoint center, nfloat radius, nfloat startAngle, nfloat endAngle);

		[Export ("appendBezierPathWithArcFromPoint:toPoint:radius:")]
		void AppendPathWithArc (CGPoint point1, CGPoint point2, nfloat radius);

		[Export ("appendBezierPathWithGlyph:inFont:")]
		void AppendPathWithGlyph (nuint glyph, NSFont font);

		[Export ("appendBezierPathWithGlyphs:count:inFont:"), Internal]
		void _AppendPathWithGlyphs (IntPtr glyphs, nint count, NSFont font);

		//IntPtr is exposed because the packedGlyphs should be treated as a "black box"
		[Export ("appendBezierPathWithPackedGlyphs:")]
		void AppendPathWithPackedGlyphs (IntPtr packedGlyphs);

		[Export ("appendBezierPathWithRoundedRect:xRadius:yRadius:")]
		void AppendPathWithRoundedRect (CGRect rect, nfloat xRadius, nfloat yRadius);

		[Export ("containsPoint:")]
		bool Contains (CGPoint point);

		//Detected properties
		[Static]
		[Export ("defaultMiterLimit")]
		nfloat DefaultMiterLimit { get; set; }

		[Static]
		[Export ("defaultFlatness")]
		nfloat DefaultFlatness { get; set; }

		[Static]
		[Export ("defaultWindingRule")]
		NSWindingRule DefaultWindingRule { get; set; }

		[Static]
		[Export ("defaultLineCapStyle")]
		NSLineCapStyle DefaultLineCapStyle { get; set; }

		[Static]
		[Export ("defaultLineJoinStyle")]
		NSLineJoinStyle DefaultLineJoinStyle { get; set; }

		[Static]
		[Export ("defaultLineWidth")]
		nfloat DefaultLineWidth { get; set; }

		[Export ("lineWidth")]
		nfloat LineWidth { get; set; }

		[Export ("lineCapStyle")]
		NSLineCapStyle LineCapStyle { get; set; }

		[Export ("lineJoinStyle")]
		NSLineJoinStyle LineJoinStyle { get; set; }

		[Export ("windingRule")]
		NSWindingRule WindingRule { get; set; }

		[Export ("miterLimit")]
		nfloat MiterLimit { get; set; }

		[Export ("flatness")]
		nfloat Flatness { get; set; }
	}


	[BaseType (typeof (NSImageRep))]
	[DisableDefaultCtor] // An uncaught exception was raised: -[NSBitmapImageRep init]: unrecognized selector sent to instance 0x686880
	public partial interface NSBitmapImageRep {
		[Export ("initWithFocusedViewRect:")]
		IntPtr Constructor (CGRect rect);

		[Export ("initWithBitmapDataPlanes:pixelsWide:pixelsHigh:bitsPerSample:samplesPerPixel:hasAlpha:isPlanar:colorSpaceName:bytesPerRow:bitsPerPixel:")]
		IntPtr Constructor (IntPtr planes, nint width, nint height, nint bps, nint spp, bool alpha, bool isPlanar,
				    string colorSpaceName, nint rBytes, nint pBits);

		[Export ("initWithBitmapDataPlanes:pixelsWide:pixelsHigh:bitsPerSample:samplesPerPixel:hasAlpha:isPlanar:colorSpaceName:bitmapFormat:bytesPerRow:bitsPerPixel:")]
		IntPtr Constructor (IntPtr planes, nint width, nint height, nint bps, nint spp, bool alpha, bool isPlanar, string colorSpaceName,
				    NSBitmapFormat bitmapFormat, nint rBytes, nint pBits);

		[Export ("initWithCGImage:")]
		IntPtr Constructor (CGImage cgImage);

		[Export ("initWithCIImage:")]
		IntPtr Constructor (MonoMac.CoreImage.CIImage ciImage);

		[Static]
		[Export ("imageRepsWithData:")]
		NSImageRep [] ImageRepsWithData (NSData data);

		[Static]
		[Export ("imageRepWithData:")]
		NSImageRep ImageRepFromData (NSData data);

		[Export ("initWithData:")]
		IntPtr Constructor (NSData data);

		[Export ("bitmapData")]
		IntPtr BitmapData { get; }

		[Export ("getBitmapDataPlanes:")]
		void GetBitmapDataPlanes (IntPtr data);

		[Export ("isPlanar")]
		bool IsPlanar { get; }

		[Export ("samplesPerPixel")]
		nint SamplesPerPixel { get; }

		[Export ("bitsPerPixel")]
		nint BitsPerPixel { get; }

		[Export ("bytesPerRow")]
		nint BytesPerRow { get; }

		[Export ("bytesPerPlane")]
		nint BytesPerPlane { get; }

		[Export ("numberOfPlanes")]
		nint Planes { get; }

		[Export ("bitmapFormat")]
		NSBitmapFormat BitmapFormat { get; }

		[Export ("getCompression:factor:")]
		void GetCompressionFactor (out NSTiffCompression compression, out nfloat factor);

		[Export ("setCompression:factor:")]
		void SetCompressionFactor (NSTiffCompression compression, nfloat factor);

		[Export ("TIFFRepresentation")]
		NSData TiffRepresentation { get; }

		[Export ("TIFFRepresentationUsingCompression:factor:")]
		NSData TiffRepresentationUsingCompressionFactor (NSTiffCompression comp, nfloat factor);

		[Static]
		[Export ("TIFFRepresentationOfImageRepsInArray:")]
		NSData ImagesAsTiff (NSImageRep [] imageReps);

		[Static]
		[Export ("TIFFRepresentationOfImageRepsInArray:usingCompression:factor:")]
		NSData ImagesAsTiff (NSImageRep [] imageReps, NSTiffCompression comp, nfloat factor);

		// FIXME: binding
		//[Static]
		//[Export ("getTIFFCompressionTypes:count:")]
		//void GetTiffCompressionTypes (const NSTIFFCompression list, int numTypes);

		[Static]
		[Export ("localizedNameForTIFFCompressionType:")]
		string LocalizedNameForTiffCompressionType (NSTiffCompression compression);

		[Export ("canBeCompressedUsing:")]
		bool CanBeCompressedUsing (NSTiffCompression compression);

		[Export ("colorizeByMappingGray:toColor:blackMapping:whiteMapping:")]
		void Colorize (nfloat midPoint, NSColor midPointColor, NSColor shadowColor, NSColor lightColor);

		[Export ("incrementalLoadFromData:complete:")]
		nint IncrementalLoad (NSData data, bool complete);

		[Export ("setColor:atX:y:")]
		void SetColorAt (NSColor color, nint x, nint y);

		[Export ("colorAtX:y:")]
		NSColor ColorAt (nint x, nint y);

		// FIXME: BINDING
		//[Export ("getPixel:atX:y:")]
		//void GetPixel (int[] p, int x, int y);
		//[Export ("setPixel:atX:y:")]
		//void SetPixel (int[] p, int x, int y);

		[Export ("CGImage")]
		CGImage CGImage { get; }

		[Export ("colorSpace")]
		NSColorSpace ColorSpace { get; }

		[Export ("bitmapImageRepByConvertingToColorSpace:renderingIntent:")]
		NSBitmapImageRep ConvertingToColorSpace (NSColorSpace targetSpace, NSColorRenderingIntent renderingIntent);

		[Export ("bitmapImageRepByRetaggingWithColorSpace:")]
		NSBitmapImageRep RetaggedWithColorSpace (NSColorSpace newSpace);
		
		[Export ("representationUsingType:properties:")]
		NSData RepresentationUsingTypeProperties(NSBitmapImageFileType storageType, [NullAllowed] NSDictionary properties);

		[Field ("NSImageCompressionMethod")]
		NSString CompressionMethod { get; }

		[Field ("NSImageCompressionFactor")]
		NSString CompressionFactor { get; }

		[Field ("NSImageDitherTransparency")]
		NSString DitherTransparency { get; }

		[Field ("NSImageRGBColorTable")]
		NSString RGBColorTable { get; }

		[Field ("NSImageInterlaced")]
		NSString Interlaced { get; }

		[Field ("NSImageColorSyncProfileData")]
		NSString ColorSyncProfileData { get; }

		[Field ("NSImageFrameCount")]
		NSString FrameCount { get; }

		[Field ("NSImageCurrentFrame")]
		NSString CurrentFrame { get; }

		[Field ("NSImageCurrentFrameDuration")]
		NSString CurrentFrameDuration { get; }

		[Field ("NSImageLoopCount")]
		NSString LoopCount { get; }

		[Field ("NSImageGamma")]
		NSString Gamma { get; }

		[Field ("NSImageProgressive")]
		NSString Progressive { get; }

		[Field ("NSImageEXIFData")]
		NSString EXIFData { get; }

		[Field ("NSImageFallbackBackgroundColor")]
		NSString FallbackBackgroundColor { get; }
	}

	[BaseType (typeof (NSView))]
	public interface NSBox {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("borderType")]
		NSBorderType BorderType { get; set; }
	
		[Export ("titlePosition")]
		NSTitlePosition TitlePosition { get; set; }
	
		[Export ("boxType")]
		NSBoxType BoxType { get; set; }
	
		[Export ("title")]
		string Title { get; set; }
	
		[Export ("titleFont")]
		NSFont TitleFont { get; set; }
	
		[Export ("borderRect")]
		CGRect BorderRect { get; } 
	
		[Export ("titleRect")]
		CGRect TitleRect { get; }
	
		[Export ("titleCell")]
		NSObject TitleCell { get; }
	
		[Export ("sizeToFit")]
		void SizeToFit ();
	
		[Export ("contentViewMargins")]
		CGSize ContentViewMargins { get; set; } 
	
		[Export ("setFrameFromContentFrame:")]
		void SetFrameFromContentFrame (CGRect contentFrame);
	
		[Export ("contentView")]
		NSObject ContentView { get; set; }
	
		[Export ("transparent")]
		bool Transparent { [Bind ("isTransparent")] get; set; }

		[Export ("setTitleWithMnemonic:")]
		void SetTitleWithMnemonic (string stringWithMnemonic);

		[Export ("borderWidth")]
		nfloat BorderWidth { get; set; }
	
		[Export ("cornerRadius")]
		nfloat CornerRadius { get; set; }
	
		[Export ("borderColor")]
		NSColor BorderColor { get; set; }
	
		[Export ("fillColor")]
		NSColor FillColor { get; set; }
	}
		
	[BaseType (typeof (NSControl))]
		// , Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSBrowserDelegate)})]
	public partial interface NSBrowser {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("loadColumnZero")]
		void LoadColumnZero ();

		[Export ("isLoaded")]
		bool Loaded { get; }

		[Export ("autohidesScroller")]
		bool AutohidesScroller  { get; set; }

		[Export ("itemAtIndexPath:")]
		NSObject ItemAtIndexPath (NSIndexPath indexPath);

		[Export ("itemAtRow:inColumn:")]
		NSObject GetItem (nint row, nint column);

		[Export ("indexPathForColumn:")]
		NSIndexPath IndexPathForColumn (nint column);

		[Export ("isLeafItem:")]
		bool IsLeafItem (NSObject item);

		[Export ("reloadDataForRowIndexes:inColumn:")]
		void ReloadData (NSIndexSet rowIndexes, nint column);

		[Export ("parentForItemsInColumn:")]
		NSObject ParentForItems (nint column);

		[Export ("scrollRowToVisible:inColumn:")]
		void ScrollRowToVisible (nint row, nint column);

		[Export ("setTitle:ofColumn:")]
		void SetTitle (string aString, nint column);

		[Export ("titleOfColumn:")]
		string ColumnTitle (nint column);

		[Export ("pathToColumn:")]
		string ColumnPath (nint column);

		[Export ("clickedColumn")]
		nint ClickedColumn ();

		[Export ("clickedRow")]
		nint ClickedRow ();

		[Export ("selectedColumn")]
		nint SelectedColumn ();

		[Export ("selectedCell")]
		NSObject SelectedCell ();

		[Export ("selectedCellInColumn:")]
		NSObject SelectedCellInColumn (nint column);

		[Export ("selectedCells")]
		NSCell [] SelectedCells ();

		[Export ("selectRow:inColumn:")]
		void Select (nint row, nint column);

		[Export ("selectedRowInColumn:")]
		nint SelectedRow (nint column);

		[Export ("selectionIndexPath")]
		NSIndexPath SelectionIndexPath { get; set; }

		[Export ("selectionIndexPaths")]
		NSIndexPath [] SelectionIndexPaths  { get; set; }

		[Export ("selectRowIndexes:inColumn:")]
		void SelectRowIndexes (NSIndexSet indexes, nint column);

		[Export ("selectedRowIndexesInColumn:")]
		NSIndexSet SelectedRowIndexes (nint column);

		[Export ("reloadColumn:")]
		void ReloadColumn (nint column);

		[Export ("validateVisibleColumns")]
		void ValidateVisibleColumns ();

		[Export ("scrollColumnsRightBy:")]
		void ScrollColumnsRightBy (nint shiftAmount);

		[Export ("scrollColumnsLeftBy:")]
		void ScrollColumnsLeftBy (nint shiftAmount);

		[Export ("scrollColumnToVisible:")]
		void ScrollColumnToVisible (nint column);

		[Export ("addColumn")]
		void AddColumn ();

		[Export ("numberOfVisibleColumns")]
		nint VisibleColumns { get; }

		[Export ("firstVisibleColumn")]
		nint FirstVisibleColumn { get; }

		[Export ("lastVisibleColumn")]
		nint LastVisibleColumn { get; }

		[Export ("columnOfMatrix:")]
		nint ColumnOfMatrix (NSMatrix matrix);

		[Export ("matrixInColumn:")]
		NSMatrix MatrixInColumn (nint column);

		[Export ("loadedCellAtRow:column:")]
		NSCell LoadedCell (nint row, nint col);

		[Export ("selectAll:")]
		void SelectAll (NSObject sender);

		[Export ("tile")]
		void Tile ();

		[Export ("doClick:")]
		void DoClick (NSObject sender);

		[Export ("doDoubleClick:")]
		void DoDoubleClick (NSObject sender);

		[Export ("sendAction")]
		bool SendAction ();

		[Export ("titleFrameOfColumn:")]
		CGRect TitleFrameOfColumn (nint column);

		[Export ("drawTitleOfColumn:inRect:")]
		void DrawTitle (nint column, CGRect aRect);

		[Export ("titleHeight")]
		nfloat TitleHeight { get; }

		[Export ("frameOfColumn:")]
		CGRect ColumnFrame (nint column);

		[Export ("frameOfInsideOfColumn:")]
		CGRect ColumnInsideFrame (nint column);

		[Export ("frameOfRow:inColumn:")]
		CGRect RowFrame (nint row, nint column);

		[Export ("getRow:column:forPoint:")]
		bool GetRowColumnForPoint (out nint row, out nint column, CGPoint point);

		[Export ("columnWidthForColumnContentWidth:")]
		nfloat ColumnWidthForColumnContentWidth (nfloat columnContentWidth);

		[Export ("columnContentWidthForColumnWidth:")]
		nfloat ColumnContentWidthForColumnWidth (nfloat columnWidth);

		[Export ("setColumnResizingType:")]
		void SetColumnResizingType (NSBrowserColumnResizingType columnResizingType);

		[Export ("columnResizingType")]
		NSBrowserColumnResizingType ColumnResizingType { get; set; }

		[Export ("prefersAllColumnUserResizing")]
		bool PrefersAllColumnUserResizing { get; set; }

		[Export ("setWidth:ofColumn:")]
		void SetColumnWidth (nfloat columnWidth, nint columnIndex);

		[Export ("widthOfColumn:")]
		nfloat GetColumnWidth (nint column);

		[Export ("rowHeight")]
		nfloat RowHeight { get; set; }

		[Export ("noteHeightOfRowsWithIndexesChanged:inColumn:")]
		void NoteHeightOfRows (NSIndexSet indexSet, nint columnIndex);

		[Export ("defaultColumnWidth")]
		nfloat DefaultColumnWidth { get; set; }

		[Export ("columnsAutosaveName")]
		string ColumnsAutosaveName  { get; set; }

		[Static]
		[Export ("removeSavedColumnsWithAutosaveName:")]
		void RemoveSavedColumnsWithAutosaveName (string name);

		[Export ("canDragRowsWithIndexes:inColumn:withEvent:")]
		bool CanDragRowsWithIndexes (NSIndexSet rowIndexes, nint column, NSEvent theEvent);

		// FIXME: binding, CGPointPointer
		//[Export ("draggingImageForRowsWithIndexes:inColumn:withEvent:offset:")]
		//NSImage DraggingImageForRowsWithIndexes (NSIndexSet rowIndexes, int column, NSEvent theEvent, CGPointPointer dragImageOffset);

		[Export ("setDraggingSourceOperationMask:forLocal:")]
		void SetDraggingSourceOperationMask (NSDragOperation mask, bool isLocal);

		[Export ("allowsTypeSelect")]
		bool AllowsTypeSelect  { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor  { get; set; }

		[Export ("editItemAtIndexPath:withEvent:select:")]
		void EditItemAtIndexPath (NSIndexPath indexPath, NSEvent theEvent, bool select);

		//Detected properties
		[Export ("doubleAction")]
		Selector DoubleAction { get; set; }

		[Export ("matrixClass")]
		Class MatrixClass { get; set; }

		[Static]
		[Export ("cellClass")]
		Class CellClass { get; }

		[Export ("setCellClass:")]
		void SetCellClass (Class factoryId);

		[Export ("cellPrototype")]
		NSObject CellPrototype { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSBrowserDelegate Delegate { get; set; }

		[Export ("reusesColumns")]
		bool ReusesColumns { get; set; }

		[Export ("hasHorizontalScroller")]
		bool HasHorizontalScroller { get; set; }

		[Export ("separatesColumns")]
		bool SeparatesColumns { get; set; }

		[Export ("titled")]
		bool Titled { [Bind ("isTitled")]get; set; }

		[Export ("minColumnWidth")]
		nfloat MinColumnWidth { get; set; }

		[Export ("maxVisibleColumns")]
		nint MaxVisibleColumns { get; set; }

		[Export ("allowsMultipleSelection")]
		bool AllowsMultipleSelection { get; set; }

		[Export ("allowsBranchSelection")]
		bool AllowsBranchSelection { get; set; }

		[Export ("allowsEmptySelection")]
		bool AllowsEmptySelection { get; set; }

		[Export ("takesTitleFromPreviousColumn")]
		bool TakesTitleFromPreviousColumn { get; set; }

		[Export ("sendsActionOnArrowKeys")]
		bool SendsActionOnArrowKeys { get; set; }

		[Export ("pathSeparator")]
		string PathSeparator { get; set; }

		[Export ("path"), Protected]
		string GetPath ();

		[Export ("setPath:"), Protected]
		bool SetPath (string path);

		[Export ("lastColumn")]
		nint LastColumn { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSBrowserDelegate {
		[Export ("browser:numberOfRowsInColumn:"), EventArgs ("NSBrowserColumn")]
		nint RowsInColumn (NSBrowser sender, nint column);

		[Export ("browser:createRowsForColumn:inMatrix:")]
		void CreateRowsForColumn (NSBrowser sender, nint column, NSMatrix matrix);

		[Export ("browser:numberOfChildrenOfItem:")]
		nint CountChildren (NSBrowser browser, NSObject item);

		[Export ("browser:child:ofItem:")]
		NSObject GetChild (NSBrowser browser, nint index, NSObject item);

		[Export ("browser:isLeafItem:")]
		bool IsLeafItem (NSBrowser browser, NSObject item);

		[Export ("browser:objectValueForItem:")]
		NSObject ObjectValueForItem (NSBrowser browser, NSObject item);

		[Export ("browser:heightOfRow:inColumn:")]
		nfloat RowHeight (NSBrowser browser, nint row, nint columnIndex);

		[Export ("rootItemForBrowser:")]
		NSObject RootItemForBrowser (NSBrowser browser);

		[Export ("browser:setObjectValue:forItem:")]
		void SetObjectValue (NSBrowser browser, NSObject obj, NSObject item);

		[Export ("browser:shouldEditItem:")]
		bool ShouldEditItem (NSBrowser browser, NSObject item);

		[Export ("browser:willDisplayCell:atRow:column:")]
		void WillDisplayCell (NSBrowser sender, NSObject cell, nint row, nint column);

		[Export ("browser:titleOfColumn:")]
		string ColumnTitle (NSBrowser sender, nint column);

		[Export ("browser:selectCellWithString:inColumn:")]
		bool SelectCellWithString (NSBrowser sender, string title, nint column);

		[Export ("browser:selectRow:inColumn:")]
		bool SelectRowInColumn (NSBrowser sender, nint row, nint column);

		[Export ("browser:isColumnValid:")]
		bool IsColumnValid (NSBrowser sender, nint column);

		[Export ("browserWillScroll:")]
		void WillScroll (NSBrowser sender);

		[Export ("browserDidScroll:")]
		void DidScroll (NSBrowser sender);

		[Export ("browser:shouldSizeColumn:forUserResize:toWidth:")]
		nfloat ShouldSizeColumn (NSBrowser browser, nint columnIndex, bool userResize, nfloat suggestedWidth);

		[Export ("browser:sizeToFitWidthOfColumn:")]
		nfloat SizeToFitWidth (NSBrowser browser, nint columnIndex);

		[Export ("browserColumnConfigurationDidChange:")]
		void ColumnConfigurationDidChange (NSNotification notification);

		[Export ("browser:shouldShowCellExpansionForRow:column:")]
		bool ShouldShowCellExpansion (NSBrowser browser, nint row, nint column);

		[Export ("browser:writeRowsWithIndexes:inColumn:toPasteboard:")]
		bool WriteRowsWithIndexesToPasteboard (NSBrowser browser, NSIndexSet rowIndexes, nint column, NSPasteboard pasteboard);

		[Export ("browser:namesOfPromisedFilesDroppedAtDestination:forDraggedRowsWithIndexes:inColumn:")]
		string [] PromisedFilesDroppedAtDestination (NSBrowser browser, NSUrl dropDestination, NSIndexSet rowIndexes, nint column);

		[Export ("browser:canDragRowsWithIndexes:inColumn:withEvent:")]
		bool CanDragRowsWithIndexes (NSBrowser browser, NSIndexSet rowIndexes, nint column, NSEvent theEvent);

		// FIXME: NSPOintPointer is a pointer to a CGPoint, so we need to support refs
		//[Export ("browser:draggingImageForRowsWithIndexes:inColumn:withEvent:offset:")]
		//NSImage DraggingImageForRowsWithIndexes (NSBrowser browser, NSIndexSet rowIndexes, int column, NSEvent theEvent, CGPointPointer dragImageOffset);

		[Export ("browser:validateDrop:proposedRow:column:dropOperation:")]
		//FIXME: NSBrowserDropOperation is also a ref (in/out) parameter
		NSDragOperation ValidateDrop (NSBrowser browser, NSDraggingInfo info, ref nint row, ref nint column, NSBrowserDropOperation dropOperation);

		[Export ("browser:acceptDrop:atRow:column:dropOperation:")]
		bool AcceptDrop (NSBrowser browser, NSDraggingInfo info, nint row, nint column, NSBrowserDropOperation dropOperation);

		[Export ("browser:typeSelectStringForRow:inColumn:")]
		string TypeSelectString (NSBrowser browser, nint row, nint column);

		[Export ("browser:shouldTypeSelectForEvent:withCurrentSearchString:")]
		bool ShouldTypeSelectForEvent (NSBrowser browser, NSEvent theEvent, string currentSearchString);

		[Export ("browser:nextTypeSelectMatchFromRow:toRow:inColumn:forString:")]
		nint NextTypeSelectMatch (NSBrowser browser, nint startRow, nint endRow, nint column, string searchString);

		[Export ("browser:previewViewControllerForLeafItem:")]
		NSViewController PreviewViewControllerForLeafItem (NSBrowser browser, NSObject item);

		[Export ("browser:headerViewControllerForItem:")]
		NSViewController HeaderViewControllerForItem (NSBrowser browser, NSObject item);

		[Export ("browser:didChangeLastColumn:toColumn:")]
		void DidChangeLastColumn (NSBrowser browser, nint oldLastColumn, nint toColumn);

		[Export ("browser:selectionIndexesForProposedSelection:inColumn:")]
		NSIndexSet SelectionIndexesForProposedSelection (NSBrowser browser, NSIndexSet proposedSelectionIndexes, nint inColumn);

	}

	[BaseType (typeof (NSCell))]
	public interface NSBrowserCell {
		[Static]
		[Export ("branchImage")]
		NSImage BranchImage { get; }

		[Static]
		[Export ("highlightedBranchImage")]
		NSImage HighlightedBranchImage { get; }

		[Export ("highlightColorInView:")]
		NSColor HighlightColorInView (NSView controlView);

		[Export ("reset")]
		void Reset ();

		[Export ("set")]
		void Set ();

		//Detected properties
		[Export ("leaf")]
		bool Leaf { [Bind ("isLeaf")]get; set; }

		[Export ("loaded")]
		bool Loaded { [Bind ("isLoaded")]get; set; }

		[Export ("image")]
		NSImage Image { get; set; }

		[Export ("alternateImage")]
		NSImage AlternateImage { get; set; }

	}

	[BaseType (typeof (NSActionCell))]
	public interface NSButtonCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("title")]
		string Title { get; set; }
	
		[Export ("alternateTitle")]
		string AlternateTitle { get; set; }
	
		[Export ("alternateImage")]
		NSImage AlternateImage { get; set; }
	
		[Export ("imagePosition")]
		NSCellImagePosition ImagePosition { get; set; }
	
		[Export ("imageScaling")]
		NSImageScale ImageScale { get; set; }
	
		[Export ("highlightsBy")]
		nint HighlightsBy { get; set; }
	
		[Export ("showsStateBy")]
		nint ShowsStateBy { get; set; }
	
		[Export ("setShowsStateBy:")]
		void SetShowsStateBy (nint aType);
	
		[Export ("setButtonType:")]
		void SetButtonType (NSButtonType aType);
	
		[Export ("isOpaque")]
		bool IsOpaque { get; }
	
		[Export ("setFont:")]
		void SetFont (NSFont  fontObj);
	
		[Export ("transparent")]
		bool Transparent { [Bind ("isTransparent")] get; set; }
	
		[Export ("setPeriodicDelay:interval:")]
		void SetPeriodicDelay (float delay, float interval); // 32-bit
	
		[Export ("getPeriodicDelay:interval:")]
		void GetPeriodicDelay (out float  delay, out float  interval); // 32-bit
	
		[Export ("keyEquivalent")]
		string KeyEquivalent { get; set; }
	
		[Export ("keyEquivalentModifierMask")]
		NSEventModifierMask KeyEquivalentModifierMask { get; set; }
	
		[Export ("keyEquivalentFont")]
		NSFont KeyEquivalentFont { get; set; }
	
		[Export ("setKeyEquivalentFont:size:")]
		void SetKeyEquivalentFont (string  fontName, nfloat fontSize);
	
		[Export ("performClick:")]
		void PerformClick (NSObject sender);
	
		[Export ("drawImage:withFrame:inView:")]
		void DrawImage (NSImage image, CGRect frame, NSView controlView);
	
		[Export ("drawTitle:withFrame:inView:")]
		CGRect DrawTitle (NSAttributedString title, CGRect frame, NSView controlView);
	
		[Export ("drawBezelWithFrame:inView:")]
		void DrawBezelWithFrame (CGRect frame, NSView controlView);

		[Export ("alternateMnemonicLocation")]
		int AlternateMnemonicLocation { get; set; }
	
		[Export ("alternateMnemonic")]
		string AlternateMnemonic { get; [Bind ("setAlternateTitleWithMnemonic:")] set; }
	
		[Export ("setGradientType:")]
		void SetGradientType (NSGradientType type);
	
		[Export ("imageDimsWhenDisabled")]
		bool ImageDimsWhenDisabled { get; set; }
	
		[Export ("showsBorderOnlyWhileMouseInside")]
		bool ShowsBorderOnlyWhileMouseInside { get; set; }
	
		[Export ("mouseEntered:")]
		void MouseEntered (NSEvent theEvent);
	
		[Export ("mouseExited:")]
		void MouseExited (NSEvent theEvent);
	
		[Export ("backgroundColor"), NullAllowed]
		NSColor BackgroundColor { get; set; }

		[Export ("attributedTitle")]
		NSAttributedString AttributedTitle { get; set; }
	
		[Export ("attributedAlternateTitle")]
		NSAttributedString AttributedAlternateTitle { get; set; }
	
		[Export ("bezelStyle")]
		NSBezelStyle BezelStyle { get; set; }

		[Export ("sound")]
		NSSound Sound { get; set; }
	
	}

	// 64 Bit reviewed
	[BaseType (typeof (NSControl))]
	public interface NSButton {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("title")]
		string Title { get; set; } 
	
		[Export ("alternateTitle")]
		string AlternateTitle { get; set; }
	
		[Export ("image"), NullAllowed]
		NSImage Image { get; set; }
	
		[Export ("alternateImage"), NullAllowed]
		NSImage AlternateImage  { get; set; }
	
		[Export ("imagePosition")]
		NSCellImagePosition ImagePosition  { get; set; }
	
		[Export ("setButtonType:")]
		void SetButtonType (NSButtonType aType);
	
		[Export ("state")]
		NSCellStateValue State { get; set; }
	
		[Export ("bordered")]
		bool Bordered  { [Bind ("isBordered")] get; set; }
	
		[Export ("transparent")]
		bool Transparent  { [Bind ("isTransparent")] get; set; }
	
		[Export ("setPeriodicDelay:interval:")]
		void SetPeriodicDelay (float delay, float interval); // 32-bit
	
		[Export ("getPeriodicDelay:interval:")]
		void GetPeriodicDelay (ref float delay, ref float interval); // 32-bit
	
		[Export ("keyEquivalent")]
		string KeyEquivalent  { get; set; }
	
		[Export ("keyEquivalentModifierMask")]
		NSEventModifierMask KeyEquivalentModifierMask  { get; set; }
	
		[Export ("highlight:")]
		void Highlight (bool flag);
	
		[Export ("performKeyEquivalent:")]
		bool PerformKeyEquivalent (NSEvent  key);

		[Export ("setTitleWithMnemonic:")]
		void SetTitleWithMnemonic (string mnemonic);

		[Export ("attributedTitle")]
		NSAttributedString AttributedTitle { get; set; }

		[Export ("attributedAlternateTitle")]
		NSAttributedString AttributedAlternateTitle  { get; set; }

		[Export ("bezelStyle")]
		NSBezelStyle BezelStyle { get; set; }

		[Export ("allowsMixedState")]
		bool AllowsMixedState { get; set;}
		
		[Export ("setNextState")]
		void SetNextState ();

		[Export ("showsBorderOnlyWhileMouseInside")]
		bool ShowsBorderOnlyWhileMouseInside ();

		[Export ("sound")]
		NSSound Sound { get; set; }
	}
	
	[BaseType (typeof (NSImageRep))]
	[DisableDefaultCtor] // An uncaught exception was raised: -[NSCachedImageRep init]: unrecognized selector sent to instance 0x14890e0
	public interface NSCachedImageRep {
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		[Export ("initWithIdentifier:")]
	   	IntPtr Constructor (NSWindow win, CGRect rect);
		
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		[Export ("initWithSize:depth:separate:alpha:")]
		IntPtr Constructor (CGSize size, NSWindowDepth depth, bool separate, bool alpha);

		[Deprecated (PlatformName.MacOSX, 10, 6)]
		[Export ("window")]
		NSWindow Window { get; }

		[Deprecated (PlatformName.MacOSX, 10, 6)]
		[Export ("rect")]
		CGRect Rectangle { get; }
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSCell : NSUserInterfaceItemIdentification {
		[Static, Export ("prefersTrackingUntilMouseUp")]
		bool PrefersTrackingUntilMouseUp { get; }
	
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);
	
		[Export ("controlView")]
		NSView ControlView { get; set; }
	
		[Export ("type")]
		NSCellType CellType { get; set; }
	
		[Export ("state")]
		NSCellStateValue State { get; set; }
	
		[Export ("target"), NullAllowed]
		NSObject Target { get; set; }
	
		[Export ("action"), NullAllowed]
		Selector Action { get; set; }
	
		[Export ("tag")]
		nint Tag { get; set; }
	
		[Export ("title")]
		string Title { get; set; }
	
		[Export ("isOpaque")]
		bool IsOpaque { get; } 
	
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }
	
		[Export ("sendActionOn:")]
		nint SendActionOn (NSEventType mask);
	
		[Export ("continuous")]
		bool IsContinuous { [Bind ("isContinuous")] get; set; }
	
		[Export ("editable")]
		bool Editable { [Bind ("isEditable")] get; set; }
	
		[Export ("selectable")]
		bool Selectable { [Bind ("isSelectable")] get; set; }
	
		[Export ("bordered")]
		bool Bordered { [Bind ("isBordered")] get; set; }
	
		[Export ("bezeled")]
		bool Bezeled { [Bind ("isBezeled")] get; set; }
	
		[Export ("scrollable")]
		bool Scrollable { [Bind ("isScrollable")] get; set; }
	
		[Export ("highlighted")]
		bool Highlighted { [Bind ("isHighlighted")] get; set; }
	
		[Export ("alignment")]
		NSTextAlignment Alignment { get; set; }
	
		[Export ("wraps")]
		bool Wraps { get; set; }
	
		[Export ("font")]
		NSFont Font { get; set; }
	
		[Export ("isEntryAcceptable:")]
		bool IsEntryAcceptable (string  aString);
	
		[Export ("keyEquivalent")]
		string KeyEquivalent { get; }
	
		[Export ("formatter")]
		NSFormatter Formatter { get; set; }
	
		[Export ("objectValue"), NullAllowed]
		NSObject ObjectValue { get; set; }
	
		[Export ("hasValidObjectValue")]
		bool HasValidObjectValue { get; }
	
		[Export ("stringValue")]
		string StringValue { get; set; }
	
		[Export ("compare:")]
		NSComparisonResult Compare (NSObject otherCell);
	
		[Export ("intValue")]
		int IntValue { get; set; }
	
		[Export ("floatValue")]
		float FloatValue { get; set; } // 32-bit
	
		[Export ("doubleValue")]
		double DoubleValue { get; set; }
	
		[Export ("takeIntValueFrom:")]
		void TakeIntValueFrom (NSObject sender);
	
		[Export ("takeFloatValueFrom:")]
		void TakeFloatValueFrom (NSObject sender);
	
		[Export ("takeDoubleValueFrom:")]
		void TakeDoubleValueFrom (NSObject sender);
	
		[Export ("takeStringValueFrom:")]
		void TakeStringValueFrom (NSObject sender);
	
		[Export ("takeObjectValueFrom:")]
		void TakeObjectValueFrom (NSObject sender);
	
		[Export ("image")]
		NSImage Image  { get; set; }
	
		[Export ("controlTint")]
		NSControlTint ControlTint { get; set; }

		[Notification, Field ("NSControlTintDidChangeNotification")]
		NSString ControlTintChangedNotification { get; }

		[Export ("controlSize")]
		NSControlSize ControlSize { get; set; }
	
		[Export ("representedObject")]
		NSObject RepresentedObject { get; set; }
	
		[Export ("cellAttribute:")]
		nint CellAttribute (NSCellAttribute aParameter);
	
		[Export ("setCellAttribute:to:")]
		void SetCellAttribute (NSCellAttribute aParameter, nint value);
	
		[Export ("imageRectForBounds:")]
		CGRect ImageRectForBounds (CGRect theRect);
	
		[Export ("titleRectForBounds:")]
		CGRect TitleRectForBounds (CGRect theRect);
	
		[Export ("drawingRectForBounds:")]
		CGRect DrawingRectForBounds (CGRect theRect);
	
		[Export ("cellSize")]
		CGSize CellSize { get; }
	
		[Export ("cellSizeForBounds:")]
		CGSize CellSizeForBounds (CGRect bounds);
	
		[Export ("highlightColorWithFrame:inView:")]
		NSColor HighlightColor (CGRect cellFrame, NSView controlView);
	
		[Export ("calcDrawInfo:")]
		void CalcDrawInfo (CGRect aRect);
	
		[Export ("setUpFieldEditorAttributes:")]
		NSText SetUpFieldEditorAttributes (NSText textObj);
	
		[Export ("drawInteriorWithFrame:inView:")]
		void DrawInteriorWithFrame (CGRect cellFrame, NSView  inView);
	
		[Export ("drawWithFrame:inView:")]
		void DrawWithFrame (CGRect cellFrame, NSView inView);
	
		[Export ("highlight:withFrame:inView:")]
		void Highlight (bool highlight, CGRect withFrame, NSView  inView);
	
		[Export ("mouseDownFlags")]
		nint MouseDownFlags { get; }
	
		[Export ("getPeriodicDelay:interval:")]
		void GetPeriodicDelay (ref nfloat delay, ref nfloat interval);
	
		[Export ("startTrackingAt:inView:")]
		bool StartTracking (CGPoint startPoint, NSView inView);
	
		[Export ("continueTracking:at:inView:")]
		bool ContinueTracking (CGPoint lastPoint, CGPoint currentPoint, NSView inView);
	
		[Export ("stopTracking:at:inView:mouseIsUp:")]
		void StopTracking (CGPoint lastPoint, CGPoint stopPoint, NSView inView, bool mouseIsUp);
	
		[Export ("trackMouse:inRect:ofView:untilMouseUp:")]
		bool TrackMouse (NSEvent  theEvent, CGRect cellFrame, NSView  controlView, bool untilMouseUp);
	
		[Export ("editWithFrame:inView:editor:delegate:event:")]
		void EditWithFrame (CGRect aRect, NSView  inView, NSText editor, NSObject delegateObject, NSEvent theEvent);
	
		[Export ("selectWithFrame:inView:editor:delegate:start:length:")]
		void SelectWithFrame (CGRect aRect, NSView inView, NSText editor, NSObject delegateObject, nint selStart, nint selLength);
	
		[Export ("endEditing:")]
		void EndEditing (NSText textObj);
	
		[Export ("resetCursorRect:inView:")]
		void ResetCursorRect (CGRect cellFrame, NSView  inView);
	
		[Export ("menu")]
		NSMenu Menu { get; set; }
	
		[Export ("menuForEvent:inRect:ofView:")]
		NSMenu MenuForEvent (NSEvent theEvent, CGRect cellFrame, NSView  view);
	
		[Static]
		[Export ("defaultMenu")]
		NSMenu DefaultMenu { get; }
	
		[Export ("setSendsActionOnEndEditing:")]
		void SetSendsActionOnEndEditing (bool flag);
	
		[Export ("sendsActionOnEndEditing")]
		bool SendsActionOnEndEditing ();
	
		[Export ("baseWritingDirection")]
		NSWritingDirection BaseWritingDirection { get; set; }
       
		[Export ("lineBreakMode")]
		NSLineBreakMode LineBreakMode { get; set; }
	
		[Export ("allowsUndo")]
		bool AllowsUndo { get; set; }
	
		[Export ("integerValue")]
		nint IntegerValue { get; set; }
	
		[Export ("takeIntegerValueFrom:")]
		void TakeIntegerValueFrom (NSObject sender);
	
		[Export ("truncatesLastVisibleLine")]
		bool TruncatesLastVisibleLine { get; set; }
	
		[Export ("userInterfaceLayoutDirection")]
		NSUserInterfaceLayoutDirection UserInterfaceLayoutDirection { get; set; }
	
		[Export ("fieldEditorForView:")]
		NSTextView FieldEditorForView (NSView  aControlView);
	
		[Export ("usesSingleLineMode")]
		bool UsesSingleLineMode { get; set; }

		//  NSCell(NSCellAttributedStringMethods)
		[Export ("refusesFirstResponder")]
		bool RefusesFirstResponder ();
	
		[Export ("acceptsFirstResponder")]
		bool AcceptsFirstResponder ();
	
		[Export ("showsFirstResponder")]
		bool ShowsFirstResponder { get; set; }

		[Export ("mnemonicLocation")]
		nint MnemonicLocation { get; set; }
	
		[Export ("mnemonic")]
		string Mnemonic { get; }
	
		[Export ("setTitleWithMnemonic:")]
		void SetTitleWithMnemonic (string  stringWithAmpersand);
	
		[Export ("performClick:")]
		void PerformClick (NSObject sender);
	
		[Export ("focusRingType")]
		NSFocusRingType FocusRingType { get; set; }
	
		[Static, Export ("defaultFocusRingType")]
		NSFocusRingType DefaultFocusRingType { get; }
	
		[Export ("wantsNotificationForMarkedText")]
		bool WantsNotificationForMarkedText { get; [NotImplemented] set; }
	
		// NSCell(NSCellAttributedStringMethods)
		[Export ("attributedStringValue")]
		NSAttributedString AttributedStringValue { get; set; }
	
		[Export ("allowsEditingTextAttributes")]
		bool AllowsEditingTextAttributes { get; set; }
	
		[Export ("importsGraphics")]
		bool ImportsGraphics { get; set; }
       
		// NSCell(NSCellMixedState) {
		[Export ("allowsMixedState")]
		bool AllowsMixedState { get; set; }
	
		[Export ("nextState")]
		nint NextState { get; }
	
		[Export ("setNextState")]
		void SetNextState ();
	
		// 
		[Export ("hitTestForEvent:inRect:ofView:")]
		NSCellHit HitTest (NSEvent forEvent, CGRect inRect, NSView  ofView);
	
		// NSCell(NSCellExpansion) 
		[Export ("expansionFrameWithFrame:inView:")]
		CGRect ExpansionFrame (CGRect withFrame, NSView inView);
	
		[Export ("drawWithExpansionFrame:inView:")]
		void DrawWithExpansionFrame (CGRect cellFrame, NSView inView);
	
		[Export ("backgroundStyle")]
		NSBackgroundStyle BackgroundStyle { get; set; }
	
		[Export ("interiorBackgroundStyle")]
		NSBackgroundStyle InteriorBackgroundStyle { get; }
	
		[Lion, Export ("draggingImageComponentsWithFrame:inView:")]
		NSDraggingImageComponent [] GenerateDraggingImageComponents (CGRect frame, NSView view);

		[Lion, Export ("drawFocusRingMaskWithFrame:inView:")]
		void DrawFocusRing (CGRect cellFrameMask, NSView inControlView);

		[Lion, Export ("focusRingMaskBoundsForFrame:inView:")]
		CGRect GetFocusRingMaskBounds (CGRect cellFrame, NSView controlView);
	}

	[BaseType (typeof (NSImageRep))]
	[DisableDefaultCtor] // An uncaught exception was raised: -[NSCIImageRep init]: unrecognized selector sent to instance 0x1b682a0
	public interface NSCIImageRep {
		[Static]
		[Export ("imageRepWithCIImage:")]
		NSCIImageRep FromCIImage (CIImage image);

		[Export ("initWithCIImage:")]
		IntPtr Constructor (CIImage image);

		[Export ("CIImage")]
		CIImage CIImage { get; }
	}
	
	[BaseType (typeof (NSView))]
	public interface NSClipView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }
	
		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }
	
		[Export ("documentView")]
		NSView DocumentView { get; set; }
	
		[Export ("documentRect")]
		CGRect DocumentRect { get; }
	
		[Export ("documentCursor")]
		NSCursor DocumentCursor { get; set; }
	
		[Export ("documentVisibleRect")]
		CGRect DocumentVisibleRect ();
	
		[Export ("viewFrameChanged:")]
		void ViewFrameChanged (NSNotification  notification);
	
		[Export ("viewBoundsChanged:")]
		void ViewBoundsChanged (NSNotification  notification);
	
		[Export ("copiesOnScroll")]
		bool CopiesOnScroll { get; set; }
	
		[Export ("autoscroll:")]
		bool Autoscroll (NSEvent  theEvent);
	
		[Export ("constrainScrollPoint:")]
		CGPoint ConstrainScrollPoint (CGPoint newOrigin);
	
		[Export ("scrollToPoint:")]
		void ScrollToPoint (CGPoint newOrigin);

		[Export ("scrollClipView:toPoint:")]
		void ScrollClipView (NSClipView  aClipView, CGPoint aPoint);
	}

	[BaseType (typeof (NSViewController))]
	public interface NSCollectionViewItem {
		[Export ("initWithNibName:bundle:")]
		IntPtr Constructor ([NullAllowed] string nibNameOrNull, [NullAllowed] NSBundle nibBundleOrNull);

		[Export ("collectionView")]
		NSCollectionView CollectionView { get; }

		[Export ("selected")]
		bool Selected { [Bind ("isSelected")]get; set; }

		[Export ("imageView")]
		[Lion]
		NSImageView ImageView { get; set;  }

		[Export ("textField")]
		NSTextField TextField { get; set;  }

		[Export ("draggingImageComponents")]
		NSDraggingImageComponent [] DraggingImageComponents { get;  }
	}

	[BaseType (typeof (NSView))]
	public interface NSCollectionView : NSDraggingSource, NSDraggingDestination {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("isFirstResponder")]
		bool IsFirstResponder { get; } 

		[Export ("newItemForRepresentedObject:")]
		NSCollectionViewItem NewItemForRepresentedObject (NSObject obj);

		[Export ("itemAtIndex:")]
		NSCollectionViewItem ItemAtIndex (nint index);

		[Export ("frameForItemAtIndex:")]
		CGRect FrameForItemAtIndex (nint index);

		[Export ("setDraggingSourceOperationMask:forLocal:")]
		void SetDraggingSource (NSDragOperation dragOperationMask, bool localDestination);

		//[Export ("draggingImageForItemsAtIndexes:withEvent:offset:")]
		//NSImage DraggingImage (NSIndexSet itemIndexes, NSEvent evt, CGPointPointer dragImageOffset);

		//Detected properties
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Wrap ("WeakDelegate")]
		NSCollectionViewDelegate Delegate { get; set; }

		[Export ("content")]
		NSObject [] Content { get; set; }

		[Export ("selectable")]
		bool Selectable { [Bind ("isSelectable")]get; set; }

		[Export ("allowsMultipleSelection")]
		bool AllowsMultipleSelection { get; set; }

		[Export ("selectionIndexes")]
		NSIndexSet SelectionIndexes { get; set; }

		[Export ("itemPrototype")]
		NSCollectionViewItem ItemPrototype { get; set; }

		[Export ("maxNumberOfRows")]
		nint MaxNumberOfRows { get; set; }

		[Export ("maxNumberOfColumns")]
		nint MaxNumberOfColumns { get; set; }

		[Export ("minItemSize")]
		CGSize MinItemSize { get; set; }

		[Export ("maxItemSize")]
		CGSize MaxItemSize { get; set; }

		[Export ("backgroundColors"), NullAllowed]
		NSColor [] BackgroundColors { get; set; }

		[Lion]
		[Export ("frameForItemAtIndex:withNumberOfItems:")]
		CGRect FrameForItemAtIndex (nint index, nint numberOfItems);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public partial interface NSCollectionViewDelegate {
		[Export ("collectionView:canDragItemsAtIndexes:withEvent:")]
		bool CanDragItems (NSCollectionView collectionView, NSIndexSet indexes, NSEvent evt);

		[Export ("collectionView:writeItemsAtIndexes:toPasteboard:")]
		bool WriteItems (NSCollectionView collectionView, NSIndexSet indexes, NSPasteboard toPasteboard);

		[Export ("collectionView:namesOfPromisedFilesDroppedAtDestination:forDraggedItemsAtIndexes:")]
		string [] NamesOfPromisedFilesDroppedAtDestination (NSCollectionView collectionView, NSUrl dropUrl, NSIndexSet indexes);

		//[Export ("collectionView:draggingImageForItemsAtIndexes:withEvent:offset:")]
		//NSImage DraggingImageForItems (NSCollectionView collectionView, NSIndexSet indexes, NSEvent evg, CGPointPointer dragImageOffset);

		[Export ("collectionView:validateDrop:proposedIndex:dropOperation:")]
		//FIXME: NSCollectionViewDropOperation is also a ref (in/out) parameter
		NSDragOperation ValidateDrop (NSCollectionView collectionView, NSDraggingInfo draggingInfo, ref nint dropIndex, NSCollectionViewDropOperation dropOperation);

		[Export ("collectionView:acceptDrop:index:dropOperation:")]
		bool AcceptDrop (NSCollectionView collectionView, NSDraggingInfo draggingInfo, nint index, NSCollectionViewDropOperation dropOperation);
	}
	
    [ThreadSafe]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // -colorSpaceName not valid for the NSColor <NSColor: 0x1b94780>; need to first convert colorspace.
	public interface NSColor
	{
		[Static]
		[Export ("colorWithCalibratedWhite:alpha:")]
		NSColor FromCalibratedWhite (nfloat white, nfloat alpha);

		[Static]
		[Export ("colorWithCalibratedHue:saturation:brightness:alpha:")]
		NSColor FromCalibratedHsba (nfloat hue, nfloat saturation, nfloat brightness, nfloat alpha);

		[Static]
		[Export ("colorWithCalibratedRed:green:blue:alpha:")]
		NSColor FromCalibratedRgba (nfloat red, nfloat green, nfloat blue, nfloat alpha);

		[Static]
		[Export ("colorWithDeviceWhite:alpha:")]
		NSColor FromDeviceWhite (nfloat white, nfloat alpha);

		[Static]
		[Export ("colorWithDeviceHue:saturation:brightness:alpha:")]
		NSColor FromDeviceHsba (nfloat hue, nfloat saturation, nfloat brightness, nfloat alpha);

		[Static]
		[Export ("colorWithDeviceRed:green:blue:alpha:")]
		NSColor FromDeviceRgba (nfloat red, nfloat green, nfloat blue, nfloat alpha);

		[Static]
		[Export ("colorWithDeviceCyan:magenta:yellow:black:alpha:")]
		NSColor FromDeviceCymka (nfloat cyan, nfloat magenta, nfloat yellow, nfloat black, nfloat alpha);

		[Static]
		[Export ("colorWithCatalogName:colorName:")]
		NSColor FromCatalogName (string listName, string colorName);

		[Static]
		[Export ("colorWithColorSpace:components:count:"), Internal]
		NSColor _FromColorSpace (NSColorSpace space, IntPtr components, nint numberOfComponents);

		[Mac (10,9)]
		[Static, Export ("colorWithWhite:alpha:")]
		NSColor FromWhite (nfloat white, nfloat alpha);

		[Mac (10,9)]
		[Static, Export ("colorWithRed:green:blue:alpha:")]
		NSColor FromRgba (nfloat red, nfloat green, nfloat blue, nfloat alpha);

		[Mac (10,9)]
		[Static, Export ("colorWithHue:saturation:brightness:alpha:")]
		NSColor FromHsba (nfloat hue, nfloat saturation, nfloat brightness, nfloat alpha);
		
		[Static]
		[Export ("blackColor")]
		NSColor Black { get; }

		[Static]
		[Export ("darkGrayColor")]
		NSColor DarkGray { get; } 

		[Static]
		[Export ("lightGrayColor")]
		NSColor LightGray { get; }

		[Static]
		[Export ("whiteColor")]
		NSColor White { get; }

		[Static]
		[Export ("grayColor")]
		NSColor Gray { get; }

		[Static]
		[Export ("redColor")]
		NSColor Red { get; }

		[Static]
		[Export ("greenColor")]
		NSColor Green { get; }

		[Static]
		[Export ("blueColor")]
		NSColor Blue { get; }

		[Static]
		[Export ("cyanColor")]
		NSColor Cyan { get; }

		[Static]
		[Export ("yellowColor")]
		NSColor Yellow { get; }

		[Static]
		[Export ("magentaColor")]
		NSColor Magenta { get; }

		[Static]
		[Export ("orangeColor")]
		NSColor Orange { get; }

		[Static]
		[Export ("purpleColor")]
		NSColor Purple { get; }

		[Static]
		[Export ("brownColor")]
		NSColor Brown { get; }

		[Static]
		[Export ("clearColor")]
		NSColor Clear { get; }

		[Static]
		[Export ("controlShadowColor")]
		[Advice ("Use a context specific color, 'SeparatorColor'")]
		NSColor ControlShadow { get; }

		[Static]
		[Export ("controlDarkShadowColor")]
		[Advice ("Use a context specific color, 'SeparatorColor'")]
		NSColor ControlDarkShadow { get; }

		[Static]
		[Export ("controlColor")]
		NSColor Control { get; }

		[Static]
		[Export ("controlHighlightColor")]
		[Advice ("Use a context specific color, 'SeparatorColor'")]
		NSColor ControlHighlight { get; }

		[Static]
		[Export ("controlLightHighlightColor")]
		[Advice ("Use a context specific color, 'SeparatorColor'")]
		NSColor ControlLightHighlight { get; }

		[Static]
		[Export ("controlTextColor")]
		NSColor ControlText { get; }

		[Static]
		[Export ("controlBackgroundColor")]
		NSColor ControlBackground { get; }

		[Static]
		[Export ("selectedControlColor")]
		NSColor SelectedControl { get; }

		[Static]
		[Deprecated (PlatformName.MacOSX, message: "Use 'SelectedContentBackgroundColor' instead.")]
		[Export ("secondarySelectedControlColor")]
		NSColor SecondarySelectedControl { get; }

		[Static]
		[Export ("selectedControlTextColor")]
		NSColor SelectedControlText { get; }

		[Static]
		[Export ("disabledControlTextColor")]
		NSColor DisabledControlText { get; }

		[Static]
		[Export ("textColor")]
		NSColor Text { get; }

		[Static]
		[Export ("textBackgroundColor")]
		NSColor TextBackground { get; }

		[Static]
		[Export ("selectedTextColor")]
		NSColor SelectedText { get; }

		[Static]
		[Export ("selectedTextBackgroundColor")]
		NSColor SelectedTextBackground { get; }

		[Static]
		[Export ("gridColor")]
		NSColor Grid { get; }

		[Static]
		[Export ("keyboardFocusIndicatorColor")]
		NSColor KeyboardFocusIndicator { get; }

		[Static]
		[Export ("windowBackgroundColor")]
		NSColor WindowBackground { get; }

		[Static]
		[Export ("scrollBarColor")]
		[Advice ("Use 'NSScroller' instead")]
		NSColor ScrollBar { get; }

		[Static]
		[Export ("knobColor")]
		[Advice ("Use 'NSScroller' instead")]
		NSColor Knob { get; }

		[Static]
		[Export ("selectedKnobColor")]
		[Advice ("Use 'NSScroller' instead")]
		NSColor SelectedKnob { get; }

		[Static]
		[Export ("windowFrameColor")]
		[Advice ("Use 'NSVisualEffectMaterial.Title' instead")]
		NSColor WindowFrame { get; }

		[Static]
		[Export ("windowFrameTextColor")]
		NSColor WindowFrameText { get; }

		[Static]
		[Export ("selectedMenuItemColor")]
		[Advice ("Use 'NSVisualEffectMaterial.Selection' instead")]
		NSColor SelectedMenuItem { get; }

		[Static]
		[Export ("selectedMenuItemTextColor")]
		NSColor SelectedMenuItemText { get; }

		[Static]
		[Export ("highlightColor")]
		NSColor Highlight { get; }

		[Static]
		[Export ("shadowColor")]
		NSColor Shadow { get; }

		[Static]
		[Export ("headerColor")]
		[Advice ("Use 'NSVisualEffectMaterial.HeaderView' instead")]
		NSColor Header { get; }

		[Static]
		[Export ("headerTextColor")]
		NSColor HeaderText { get; }

		[Static]
		[Deprecated (PlatformName.MacOSX, message : "Use 'SelectedContentBackgroundColor' instead.")]
		[Export ("alternateSelectedControlColor")]
		NSColor AlternateSelectedControl { get; }

		[Static]
		[Export ("alternateSelectedControlTextColor")]
		NSColor AlternateSelectedControlText { get; }

		[Static]
		[Advice ("Use 'AlternatingContentBackgroundColors' instead.")]
		[Export ("controlAlternatingRowBackgroundColors")]
		NSColor [] ControlAlternatingRowBackgroundColors ();

		[Export ("highlightWithLevel:")]
		NSColor HighlightWithLevel (nfloat highlightLevel);

		[Export ("shadowWithLevel:")]
		NSColor ShadowWithLevel (nfloat shadowLevel);

		[Static]
		[Export ("colorForControlTint:")]
		[Advice ("Use 'NSColor.ControlAccentColor' instead.")]
		NSColor FromControlTint (NSControlTint controlTint);

		[Static]
		[Export ("currentControlTint")]
		NSControlTint CurrentControlTint { get; }

		[Export ("set")]
		void Set ();

		[Export ("setFill")]
		void SetFill ();

		[Export ("setStroke")]
		void SetStroke ();

		[Export ("colorSpaceName")]
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Type' and 'NSColorType' instead.")]
		string ColorSpaceName { get; }

		[Export ("colorUsingColorSpaceName:")]
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'GetColor' or 'UsingColorSpace' instead.")]
		NSColor UsingColorSpace ([NullAllowed] string colorSpaceName);

		[Export ("colorUsingColorSpaceName:device:")]
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'GetColor' or 'UsingColorSpace' instead.")]
		NSColor UsingColorSpace ([NullAllowed] string colorSpaceName, [NullAllowed] NSDictionary deviceDescription);

		[Export ("colorUsingColorSpace:")]
		NSColor UsingColorSpace (NSColorSpace colorSpace);

		[Export ("blendedColorWithFraction:ofColor:")]
		NSColor BlendedColor (nfloat fraction, NSColor color);

		[Export ("colorWithAlphaComponent:")]
		NSColor ColorWithAlphaComponent (nfloat alpha);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[Export ("catalogNameComponent")]
		string CatalogNameComponent { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[Export ("colorNameComponent")]
		string ColorNameComponent { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[Export ("localizedCatalogNameComponent")]
		string LocalizedCatalogNameComponent { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[Export ("localizedColorNameComponent")]
		string LocalizedColorNameComponent { get; }

		[Export ("redComponent")]
		nfloat RedComponent { [MarshalNativeExceptions] get; }

		[Export ("greenComponent")]
		nfloat GreenComponent { [MarshalNativeExceptions] get; }

		[Export ("blueComponent")]
		nfloat BlueComponent { [MarshalNativeExceptions] get; }

		[Export ("getRed:green:blue:alpha:")]
		void GetRgba (out nfloat red, out nfloat green, out nfloat blue, out nfloat alpha);

		[Export ("hueComponent")]
		nfloat HueComponent { [MarshalNativeExceptions] get; }

		[Export ("saturationComponent")]
		nfloat SaturationComponent { [MarshalNativeExceptions] get; }

		[Export ("brightnessComponent")]
		nfloat BrightnessComponent { [MarshalNativeExceptions] get; }

		[Export ("getHue:saturation:brightness:alpha:")]
		void GetHsba (out nfloat hue, out nfloat saturation, out nfloat brightness, out nfloat alpha);

		[Export ("whiteComponent")]
		nfloat WhiteComponent { [MarshalNativeExceptions] get; }

		[Export ("getWhite:alpha:")]
		void GetWhiteAlpha (out nfloat white, out nfloat alpha);

		[Export ("cyanComponent")]
		nfloat CyanComponent { [MarshalNativeExceptions] get; }

		[Export ("magentaComponent")]
		nfloat MagentaComponent { [MarshalNativeExceptions] get; }

		[Export ("yellowComponent")]
		nfloat YellowComponent { [MarshalNativeExceptions] get; }

		[Export ("blackComponent")]
		nfloat BlackComponent { [MarshalNativeExceptions] get; }

		[Export ("getCyan:magenta:yellow:black:alpha:")]
		void GetCmyka (out nfloat cyan, out nfloat magenta, out nfloat yellow, out nfloat black, out nfloat alpha);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[Export ("colorSpace")]
		NSColorSpace ColorSpace { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[Export ("numberOfComponents")]
		nint ComponentCount { get; }

		[Export ("getComponents:"), Internal]
		void _GetComponents (IntPtr components);

		[Export ("alphaComponent")]
		nfloat AlphaComponent { [MarshalNativeExceptions] get; }

		[Static]
		[Export ("colorFromPasteboard:")]
		NSColor FromPasteboard (NSPasteboard pasteBoard);

		[Export ("writeToPasteboard:")]
		void WriteToPasteboard (NSPasteboard pasteBoard);

		[Static]
		[Export ("colorWithPatternImage:")]
		NSColor FromPatternImage (NSImage image);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[Export ("patternImage")]
		NSImage PatternImage { get; }

		[Export ("CGColor")]
		CGColor CGColor { get; }

		[Export ("drawSwatchInRect:")]
		void DrawSwatchInRect (CGRect rect);

		[Static]
		[Export ("ignoresAlpha")]
		bool IgnoresAlpha { get; set; }

		[Static]
		[Export ("colorWithCIColor:")]
		NSColor FromCIColor (CIColor color);

		[Mac (10,10)]
		[Static, Export ("labelColor")]
		NSColor LabelColor { get; }

		[Mac (10,10)]
		[Static, Export ("secondaryLabelColor")]
		NSColor SecondaryLabelColor { get; }

		[Mac (10,10)]
		[Static, Export ("tertiaryLabelColor")]
		NSColor TertiaryLabelColor { get; } 

		[Mac (10,10)]
		[Static, Export ("quaternaryLabelColor")]
		NSColor QuaternaryLabelColor { get; }

		[Mac (10, 10)]
		[Static, Export ("linkColor", ArgumentSemantic.Strong)]
		NSColor LinkColor { get; }
		
		[Mac (10,12)]
		[Static]
		[Export ("colorWithDisplayP3Red:green:blue:alpha:")]
		NSColor FromDisplayP3 (nfloat red, nfloat green, nfloat blue, nfloat alpha);

		[Mac (10,12)]
		[Static]
		[Export ("colorWithColorSpace:hue:saturation:brightness:alpha:")]
		NSColor FromColor (NSColorSpace space, nfloat hue, nfloat saturation, nfloat brightness, nfloat alpha);

		[Mac (10, 12, 2)]
		[Static]
		[Export ("scrubberTexturedBackgroundColor", ArgumentSemantic.Strong)]
		NSColor ScrubberTexturedBackgroundColor { get; }

		[Mac (10,13)]
		[Static]
		[Export ("colorNamed:bundle:")]
		[return: NullAllowed]
		NSColor FromName (string name, [NullAllowed] NSBundle bundle);

		[Mac (10,13)]
		[Static]
		[Export ("colorNamed:")]
		[return: NullAllowed]
		NSColor FromName (string name);

		[Mac (10, 13)]
		[Export ("type")]
		NSColorType Type { get; }

		[Mac (10,13)]
		[Export ("colorUsingType:")]
		[return: NullAllowed]
		NSColor GetColor (NSColorType type);

		[Mac (10, 10)]
		[Static]
		[Export ("systemRedColor", ArgumentSemantic.Strong)]
		NSColor SystemRedColor { get; }

		[Mac (10, 10)]
		[Static]
		[Export ("systemGreenColor", ArgumentSemantic.Strong)]
		NSColor SystemGreenColor { get; }

		[Mac (10, 10)]
		[Static]
		[Export ("systemBlueColor", ArgumentSemantic.Strong)]
		NSColor SystemBlueColor { get; }

		[Mac (10, 10)]
		[Static]
		[Export ("systemOrangeColor", ArgumentSemantic.Strong)]
		NSColor SystemOrangeColor { get; }

		[Mac (10, 10)]
		[Static]
		[Export ("systemYellowColor", ArgumentSemantic.Strong)]
		NSColor SystemYellowColor { get; }

		[Mac (10, 10)]
		[Static]
		[Export ("systemBrownColor", ArgumentSemantic.Strong)]
		NSColor SystemBrownColor { get; }

		[Mac (10, 10)]
		[Static]
		[Export ("systemPinkColor", ArgumentSemantic.Strong)]
		NSColor SystemPinkColor { get; }

		[Mac (10, 10)]
		[Static]
		[Export ("systemPurpleColor", ArgumentSemantic.Strong)]
		NSColor SystemPurpleColor { get; }

		[Mac (10, 10)]
		[Static]
		[Export ("systemGrayColor", ArgumentSemantic.Strong)]
		NSColor SystemGrayColor { get; }

		[Mac (10, 14, onlyOn64: true)]
		[Static]
		[Export ("separatorColor", ArgumentSemantic.Strong)]
		NSColor Separator { get; }

		[Mac (10, 14, onlyOn64: true)]
		[Static]
		[Export ("selectedContentBackgroundColor", ArgumentSemantic.Strong)]
		NSColor SelectedContentBackground { get; }

		[Mac (10, 14, onlyOn64: true)]
		[Static]
		[Export ("unemphasizedSelectedContentBackgroundColor", ArgumentSemantic.Strong)]
		NSColor UnemphasizedSelectedContentBackground { get; }

		[Mac (10, 14, onlyOn64: true)]
		[Static]
		[Export ("alternatingContentBackgroundColors", ArgumentSemantic.Strong)]
		NSColor[] AlternatingContentBackgroundColors { get; }

		[Mac (10, 14, onlyOn64: true)]
		[Static]
		[Export ("unemphasizedSelectedTextBackgroundColor", ArgumentSemantic.Strong)]
		NSColor UnemphasizedSelectedTextBackground { get; }

		[Mac (10, 14, onlyOn64: true)]
		[Static]
		[Export ("unemphasizedSelectedTextColor", ArgumentSemantic.Strong)]
		NSColor UnemphasizedSelectedText { get; }

		[Mac (10, 14, onlyOn64: true)]
		[Static]
		[Export ("controlAccentColor", ArgumentSemantic.Strong)]
		NSColor ControlAccent { get; }

		[Mac (10,14, onlyOn64: true)]
		[Export ("colorWithSystemEffect:")]
		NSColor FromSystemEffect (NSColorSystemEffect systemEffect);

		[Mac (10, 13)]
		[Static]
		[Export ("findHighlightColor", ArgumentSemantic.Strong)]
		NSColor FindHighlight { get; }

		[Mac (10, 10)]
		[Static]
		[Export ("placeholderTextColor", ArgumentSemantic.Strong)]
		NSColor PlaceholderText { get; }

        [Static, Export ("underPageBackgroundColor")]
        NSColor UnderPageBackground { get; }

        [Static, Export ("colorWithCGColor:")]
        NSColor FromCGColor (CGColor cgColor);
	}

	[BaseType (typeof (NSObject))]
	public interface NSColorList {
		[Static]
		[Export ("availableColorLists")]
		NSColorList [] AvailableColorLists { get; }

		[Static]
		[Export ("colorListNamed:")]
		NSColorList ColorListNamed (string name);

		[Export ("initWithName:")]
		IntPtr Constructor (string name);

		[Export ("initWithName:fromFile:")]
		IntPtr Constructor (string name, [NullAllowed] string path);

		[Export ("name")]
		string Name { get; }

		[Export ("setColor:forKey:")]
		void SetColorForKey (NSColor color, string key);

		[Export ("insertColor:key:atIndex:")]
		void InsertColor (NSColor color, string key, nint indexPos);

		[Export ("removeColorWithKey:")]
		void RemoveColor (string key);

		[Export ("colorWithKey:")]
		NSColor ColorWithKey (string key);

		[Export ("allKeys")]
		string [] AllKeys ();

		[Export ("isEditable")]
		bool IsEditable { get; }

		[Export ("writeToFile:")]
		bool WriteToFile ([NullAllowed] string path);

		[Export ("removeFile")]
		void RemoveFile ();
	}

	[BaseType (typeof (NSPanel))]
	public partial interface NSColorPanel {
		[Static, Export ("sharedColorPanel")]
		NSColorPanel SharedColorPanel { get; }

		[Static]
		[Export ("sharedColorPanelExists")]
		bool SharedColorPanelExists { get; }

		[Static]
		[Export ("dragColor:withEvent:fromView:")]
		bool DragColor (NSColor color, NSEvent theEvent, NSView sourceView);

		[Static]
		[Export ("setPickerMask:")]
		void SetPickerStyle (NSColorPanelFlags mask);

		[Static]
		[Export ("setPickerMode:")]
		void SetPickerMode (NSColorPanelMode mode);

		[Export ("alpha")]
		nfloat Alpha { get; }

		[Export ("setAction:")]
		void SetAction ([NullAllowed] Selector aSelector);

		[Export ("setTarget:")]
		void SetTarget ([NullAllowed] NSObject anObject);

		[Export ("attachColorList:")]
		void AttachColorList (NSColorList colorList);

		[Export ("detachColorList:")]
		void DetachColorList (NSColorList colorList);

		//Detected properties
		[Export ("accessoryView"), NullAllowed]
		NSView AccessoryView { get; set; }

		[Export ("continuous")]
		bool Continuous { [Bind ("isContinuous")]get; set; }

		[Export ("showsAlpha")]
		bool ShowsAlpha { get; set; }

		[Export ("mode")]
		NSColorPanelFlags Mode { get; set; }

		[Export ("color")]
		NSColor Color { get; set; }

	}

	[BaseType (typeof (NSObject))]
	public interface NSColorPicker {
		[Export ("initWithPickerMask:colorPanel:")]
		IntPtr Constructor (NSColorPanelFlags mask, NSColorPanel owningColorPanel);

		[Export ("colorPanel")]
		NSColorPanel ColorPanel { get; }

		[Export ("provideNewButtonImage")]
		NSImage ProvideNewButtonImage ();

		[Export ("insertNewButtonImage:in:")]
		void InsertNewButtonImage (NSImage newButtonImage, NSButtonCell buttonCell);

		[Export ("viewSizeChanged:")]
		void ViewSizeChanged (NSObject sender);

		[Export ("attachColorList:")]
		void AttachColorList (NSColorList colorList);

		[Export ("detachColorList:")]
		void DetachColorList (NSColorList colorList);

		[Export ("setMode:")]
		void SetMode (NSColorPanelMode mode);

		[Export ("buttonToolTip")]
		string ButtonToolTip { get; }

		[Export ("minContentSize")]
		CGSize MinContentSize { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSColorSpace {
		[Export ("initWithICCProfileData:")]
		IntPtr Constructor (NSData iccData);

		[Export ("ICCProfileData")]
		NSData ICCProfileData { get; }

		// Conflicts with the built-in handle intptr
		//[Export ("initWithColorSyncProfile:")]
		//IntPtr Constructor (IntPtr colorSyncProfile);

		[Export ("colorSyncProfile")]
		IntPtr ColorSyncProfile { get; }

		[Export ("initWithCGColorSpace:")]
		IntPtr Constructor (MonoMac.CoreGraphics.CGColorSpace cgColorSpace);

		[Export ("CGColorSpace")]
		MonoMac.CoreGraphics.CGColorSpace ColorSpace { get; }

		[Export ("numberOfColorComponents")]
		int ColorComponents { get; }

		[Export ("colorSpaceModel")]
		NSColorSpaceModel ColorSpaceModel { get; }

		[Export ("localizedName")]
		string LocalizedName { get; }

		[Static]
		[Export ("genericRGBColorSpace")]
		NSColorSpace GenericRGBColorSpace { get; }

		[Static]
		[Export ("genericGrayColorSpace")]
		NSColorSpace GenericGrayColorSpace { get; }

		[Static]
		[Export ("genericCMYKColorSpace")]
		NSColorSpace GenericCMYKColorSpace { get; }

		[Static]
		[Export ("deviceRGBColorSpace")]
		NSColorSpace DeviceRGBColorSpace { get; }

		[Static]
		[Export ("deviceGrayColorSpace")]
		NSColorSpace DeviceGrayColorSpace { get; }

		[Static]
		[Export ("deviceCMYKColorSpace")]
		NSColorSpace DeviceCMYKColorSpace { get; }

		[Static]
		[Export ("sRGBColorSpace")]
		NSColorSpace SRGBColorSpace { get; }

		[Static]
		[Export ("genericGamma22GrayColorSpace")]
		NSColorSpace GenericGamma22GrayColorSpace { get; }

		[Static]
		[Export ("adobeRGB1998ColorSpace")]
		NSColorSpace AdobeRGB1998ColorSpace { get; }

		[Static]
		[Export ("availableColorSpacesWithModel:")]
		NSColorSpace [] AvailableColorSpacesWithModel (NSColorSpaceModel model);

		[Field ("NSCalibratedWhiteColorSpace")]
		NSString CalibratedWhite { get; }

		[Field ("NSCalibratedBlackColorSpace")]
		NSString CalibratedBlack { get; }
		
		[Field ("NSCalibratedRGBColorSpace")]
		NSString CalibratedRGB { get; }

		[Field ("NSDeviceWhiteColorSpace")]
		NSString DeviceWhite { get; }

		[Field ("NSDeviceBlackColorSpace")]
		NSString DeviceBlack { get; }

		[Field ("NSDeviceRGBColorSpace")]
		NSString DeviceRGB { get; }

		[Field ("NSDeviceCMYKColorSpace")]
		NSString DeviceCMYK { get; }

		[Field ("NSNamedColorSpace")]
		NSString Named { get; }

		[Field ("NSPatternColorSpace")]
		NSString Pattern { get; }

		[Field ("NSCustomColorSpace")]
		NSString Custom { get; }
	}

	[BaseType (typeof (NSControl))]
	public interface NSColorWell {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("deactivate")]
		void Deactivate ();

		[Export ("activate:")]
		void Activate (bool exclusive);

		[Export ("isActive")]
		bool IsActive { get; }

		[Export ("drawWellInside:")]
		void DrawWellInside (CGRect insideRect);

		[Export ("takeColorFrom:")]
		void TakeColorFrom (NSObject sender);

		//Detected properties
		[Export ("bordered")]
		bool Bordered { [Bind ("isBordered")]get; set; }

		[Export ("color")]
		NSColor Color { get; set; }

	}


	[BaseType (typeof (NSTextField))]
	public partial interface NSComboBox {

		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);
		
		[Export ("hasVerticalScroller")]
		bool HasVerticalScroller { get; set; }

		[Export ("intercellSpacing")]
		CGSize IntercellSpacing { get; set; }

		[Export ("itemHeight")]
		nfloat ItemHeight { get; set; }

		[Export ("numberOfVisibleItems")]
		nint VisibleItems { get; set; }

		[Export ("buttonBordered")]
		bool ButtonBordered { [Bind ("isButtonBordered")] get; set; }

		[Export ("reloadData")]
		void ReloadData ();

		[Export ("noteNumberOfItemsChanged")]
		void NoteNumberOfItemsChanged ();

		[Export ("usesDataSource")]
		bool UsesDataSource { get; set; }

		[Export ("scrollItemAtIndexToTop:")]
		void ScrollItemAtIndexToTop (nint scrollItemIndex);

		[Export ("scrollItemAtIndexToVisible:")]
		void ScrollItemAtIndexToVisible (nint scrollItemIndex);

		[Export ("selectItemAtIndex:")]
		void SelectItem (nint itemIndex);

		[Export ("deselectItemAtIndex:")]
		void DeselectItem (nint itemIndex);

		[Export ("indexOfSelectedItem")]
		nint SelectedIndex { get; }

		[Export ("numberOfItems")]
		nint Count { get; }

		[Export ("completes")]
		bool Completes { get; set; }

		[Export ("dataSource")]
		NSComboBoxDataSource DataSource { get; set; }

		[Export ("addItemWithObjectValue:")]
		void Add (NSObject object1);

		[Export ("addItemsWithObjectValues:")]
		[PostGet ("Values")]
		void Add (NSObject [] items);

		[Export ("insertItemWithObjectValue:atIndex:")]
		[PostGet ("Values")]
		void Insert (NSObject object1, nint index);

		[Export ("removeItemWithObjectValue:")]
		[PostGet ("Values")]
		void Remove (NSObject object1);

		[Export ("removeItemAtIndex:")]
		[PostGet ("Values")]
		void RemoveAt (nint index);

		[Export ("removeAllItems")]
		[PostGet ("Values")]
		void RemoveAll ();

		[Export ("selectItemWithObjectValue:")]
		void Select (NSObject object1);

		[Export ("itemObjectValueAtIndex:")]
		NSComboBox GetItem (nint index);

		[Export ("objectValueOfSelectedItem")]
		NSObject SelectedValue { get; }

		[Export ("indexOfItemWithObjectValue:")]
		nint IndexOf (NSObject object1);

		[Export ("objectValues")]
		NSObject [] Values { get; }

		[Notification, Field ("NSComboBoxSelectionDidChangeNotification")]
		NSString SelectionDidChangeNotification { get; }

		[Notification, Field ("NSComboBoxSelectionIsChangingNotification")]
		NSString SelectionIsChangingNotification { get; }

		[Notification, Field ("NSComboBoxWillDismissNotification")]
		NSString WillDismissNotification { get; }

		[Notification, Field ("NSComboBoxWillPopUpNotification")]
		NSString WillPopUpNotification { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSComboBoxDataSource {
		[Export ("comboBox:objectValueForItemAtIndex:")]
		NSObject ObjectValueForItem (NSComboBox comboBox, nint index);
		
		[Export ("numberOfItemsInComboBox:")]
		nint ItemCount (NSComboBox comboBox);
		
		[Export ("comboBox:completedString:")]
		string CompletedString (NSComboBox comboBox, string uncompletedString);
		
		[Export ("comboBox:indexOfItemWithStringValue:")]
		nint IndexOfItem (NSComboBox comboBox, string value);
	}

	[BaseType (typeof (NSTextFieldCell))]
	public partial interface NSComboBoxCell {

		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("hasVerticalScroller")]
		bool HasVerticalScroller { get; set; }

		[Export ("intercellSpacing")]
		CGSize IntercellSpacing { get; set; }

		[Export ("itemHeight")]
		nfloat ItemHeight { get; set; }

		[Export ("numberOfVisibleItems")]
		nint VisibleItems { get; set; }

		[Export ("buttonBordered")]
		bool ButtonBordered { [Bind ("isButtonBordered")] get; set; }

		[Export ("reloadData")]
		void ReloadData ();

		[Export ("noteNumberOfItemsChanged")]
		void NoteNumberOfItemsChanged ();

		[Export ("usesDataSource")]
		bool UsesDataSource { get; set; }

		[Export ("scrollItemAtIndexToTop:")]
		void ScrollItemAtIndexToTop (int scrollItemIndex);

		[Export ("scrollItemAtIndexToVisible:")]
		void ScrollItemAtIndexToVisible (int scrollItemIndex);

		[Export ("selectItemAtIndex:")]
		void SelectItem (int itemIndex);

		[Export ("deselectItemAtIndex:")]
		void DeselectItem (int itemIndex);

		[Export ("indexOfSelectedItem")]
		nint SelectedIndex { get; }

		[Export ("numberOfItems")]
		nint Count { get; }

		[Export ("completes")]
		bool Completes { get; set; }

		[Export ("dataSource")]
		NSComboBoxCellDataSource DataSource { get; set; }

		[Export ("addItemWithObjectValue:")]
		void Add (NSObject object1);

		[Export ("addItemsWithObjectValues:")]
		[PostGet ("Values")]
		void Add (NSObject [] items);

		[Export ("insertItemWithObjectValue:atIndex:")]
		[PostGet ("Values")]
		void Insert (NSObject object1, int index);

		[Export ("removeItemWithObjectValue:")]
		[PostGet ("Values")]
		void Remove (NSObject object1);

		[Export ("removeItemAtIndex:")]
		[PostGet ("Values")]
		void RemoveAt (int index);

		[Export ("removeAllItems")]
		[PostGet ("Values")]
		void RemoveAll ();

		[Export ("selectItemWithObjectValue:")]
		void Select (NSObject object1);

		[Export ("itemObjectValueAtIndex:")]
		NSComboBox GetItem (int index);

		[Export ("objectValueOfSelectedItem")]
		NSObject SelectedValue { get; }

		[Export ("indexOfItemWithObjectValue:")]
		nint IndexOf (NSObject object1);

		[Export ("objectValues")]
		NSObject [] Values { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public partial interface NSComboBoxCellDataSource {
		[Export ("comboBoxCell:objectValueForItemAtIndex:")]
		NSObject ObjectValueForItem (NSComboBoxCell comboBox, int index);

		[Export ("numberOfItemsInComboBoxCell:")]
		int ItemCount (NSComboBoxCell comboBox);

		[Export ("comboBoxCell:completedString:")]
		string CompletedString (NSComboBoxCell comboBox, string uncompletedString);

		[Export ("comboBoxCell:indexOfItemWithStringValue:")]
		uint IndexOfItem (NSComboBoxCell comboBox, string value);
	}
	
	[BaseType (typeof (NSView))]
	public partial interface NSControl {
        [DesignatedInitializer]
        [Export("initWithFrame:")]
        IntPtr Constructor(CGRect frameRect);

        [Export("sizeToFit")]
        void SizeToFit();

        [Deprecated(PlatformName.MacOSX, 10, 10, message: "Override 'Layout' instead.")]
        [Export("calcSize")]
        void CalcSize();

        [Deprecated(PlatformName.MacOSX, 10, 10)]
        [Export("selectedCell")]
        NSCell SelectedCell { get; }

        [Deprecated(PlatformName.MacOSX, 10, 10)]
        [Export("selectedTag")]
        nint SelectedTag { get; }

        [Export("sendActionOn:")]
        nint SendActionOn(NSEventType mask);

        [Deprecated(PlatformName.MacOSX, 10, 10)]
        [Export("setNeedsDisplay")]
        void SetNeedsDisplay();

        [Deprecated(PlatformName.MacOSX, 10, 10)]
        [Export("updateCell:")]
        void UpdateCell(NSCell aCell);

        [Deprecated(PlatformName.MacOSX, 10, 10)]
        [Export("updateCellInside:")]
        void UpdateCellInside(NSCell aCell);

        [Deprecated(PlatformName.MacOSX, 10, 10)]
        [Export("drawCellInside:")]
        void DrawCellInside(NSCell aCell);

        [Deprecated(PlatformName.MacOSX, 10, 10)]
        [Export("drawCell:")]
        void DrawCell(NSCell aCell);

        [Deprecated(PlatformName.MacOSX, 10, 10)]
        [Export("selectCell:")]
        void SelectCell(NSCell aCell);

        [Export("sendAction:to:")]
        bool SendAction(Selector theAction, NSObject theTarget);

        [Export("takeIntValueFrom:")]
        void TakeIntValueFrom(NSObject sender);

        [Export("takeFloatValueFrom:")]
        void TakeFloatValueFrom(NSObject sender);

        [Export("takeDoubleValueFrom:")]
        void TakeDoubleValueFrom(NSObject sender);

        [Export("takeStringValueFrom:")]
        void TakeStringValueFrom(NSObject sender);

        [Export("takeObjectValueFrom:")]
        void TakeObjectValueFrom(NSObject sender);

        [Export("currentEditor")]
        NSText CurrentEditor { get; }

        [Export("abortEditing")]
        bool AbortEditing();

        [Export("validateEditing")]
        void ValidateEditing();

        [Export("mouseDown:")]
        void MouseDown(NSEvent theEvent);

        [Export("takeIntegerValueFrom:")]
        void TakeIntegerValueFrom(NSObject sender);

        [Export("invalidateIntrinsicContentSizeForCell:")]
        void InvalidateIntrinsicContentSizeForCell(NSCell cell);

        //Detected properties
        [Deprecated(PlatformName.MacOSX, 10, 10)]
        [Static]
        [Export("cellClass")]
        Class CellClass { get; set; }

        [Deprecated(PlatformName.MacOSX, 10, 10)]
        [Export("cell")]
        NSCell Cell { get; set; }

        [Export("target", ArgumentSemantic.Weak), NullAllowed]
        NSObject Target { get; set; }

        [Export("action"), NullAllowed]
        Selector Action { get; set; }

        [Export("tag")]
        nint Tag { get; set; }

        [Export("ignoresMultiClick")]
        bool IgnoresMultiClick { get; set; }

        [Export("continuous")]
        bool Continuous { [Bind("isContinuous")]get; set; }

        [Export("enabled")]
        bool Enabled { [Bind("isEnabled")]get; set; }

        [Export("alignment")]
        NSTextAlignment Alignment { get; set; }

        [Export("font")]
        NSFont Font { get; set; }

        [Export("formatter", ArgumentSemantic.Retain), NullAllowed]
#if XAMCORE_4_0
		NSFormatter Formatter { get; set; }
#else
        NSObject Formatter { get; set; }
#endif

        [Export("objectValue", ArgumentSemantic.Copy)]
        NSObject ObjectValue { get; set; }

        [Export("stringValue")]
        string StringValue { get; set; }

        [Export("attributedStringValue", ArgumentSemantic.Copy)]
        NSAttributedString AttributedStringValue { get; set; }

        [Export("intValue")]
        int IntValue { get; set; } /* int, not NSInteger */

        [Export("floatValue")]
        float FloatValue { get; set; } /* float, not CGFloat */

        [Export("doubleValue")]
        double DoubleValue { get; set; }

        [Export("baseWritingDirection")]
        NSWritingDirection BaseWritingDirection { get; set; }

#if XAMCORE_2_0
		[Export ("integerValue")]
		nint NIntValue { get; set; }
#else
        [Export("integerValue")]
        nint IntegerValue { get; set; }
#endif
        [Export("performClick:")]
        void PerformClick(NSObject sender);

        [Export("refusesFirstResponder")]
        bool RefusesFirstResponder { get; set; }

        [Mac(10, 10)]
        [Export("highlighted")]
        bool Highlighted { [Bind("isHighlighted")] get; [Bind("setHighlighted:")] set; }

        [Mac(10, 10)]
        [Export("controlSize")]
        NSControlSize ControlSize { get; set; }

        [Mac(10, 10)]
        [Export("sizeThatFits:")]
        CGSize SizeThatFits(CGSize size);

        [Mac(10, 10)]
        [Export("lineBreakMode")]
        NSLineBreakMode LineBreakMode { get; set; }

        [Mac(10, 10)]
        [Export("usesSingleLineMode")]
        bool UsesSingleLineMode { get; set; }

        [Mac(10, 10)]
        [Export("drawWithExpansionFrame:inView:")]
        void DrawWithExpansionFrame(CGRect cellFrame, NSView view);

        [Mac(10, 10)]
        [Export("editWithFrame:editor:delegate:event:")]
        void EditWithFrame(CGRect aRect, [NullAllowed] NSText textObj, [NullAllowed] NSObject anObject, NSEvent theEvent);

        [Mac(10, 10)]
        [Export("selectWithFrame:editor:delegate:start:length:")]
        void SelectWithFrame(CGRect aRect, [NullAllowed] NSText textObj, [NullAllowed] NSObject anObject, nint selStart, nint selLength);

        [Mac(10, 10)]
        [Export("endEditing:")]
        void EndEditing([NullAllowed] NSText textObj);
    }

	[BaseType (typeof (NSObject))]
	public interface NSController {
		[Export ("objectDidBeginEditing:")]
		void ObjectDidBeginEditing (NSObject editor);

		[Export ("objectDidEndEditing:")]
		void ObjectDidEndEditing (NSObject editor);

		[Export ("discardEditing")]
		void DiscardEditing ();

		[Export ("commitEditing")]
		bool CommitEditing { get; }

		[Export ("commitEditingWithDelegate:didCommitSelector:contextInfo:")]
		void CommitEditingWithDelegate (NSObject delegate1, Selector didCommitSelector, IntPtr contextInfo);

		[Export ("isEditing")]
		bool IsEditing { get; }

	}

	[BaseType (typeof (NSObject))]
	public interface NSCursor {
		[Static]
		[Export ("currentCursor")]
		NSCursor CurrentCursor { get; }

		[Static]
		[Export ("currentSystemCursor")]
		NSCursor CurrentSystemCursor { get; }

		[Static]
		[Export ("arrowCursor")]
		NSCursor ArrowCursor { get; }

		[Static]
		[Export ("IBeamCursor")]
		NSCursor IBeamCursor { get; }

		[Static]
		[Export ("pointingHandCursor")]
		NSCursor PointingHandCursor { get; }

		[Static]
		[Export ("closedHandCursor")]
		NSCursor ClosedHandCursor { get; }

		[Static]
		[Export ("openHandCursor")]
		NSCursor OpenHandCursor { get; }

		[Static]
		[Export ("resizeLeftCursor")]
		NSCursor ResizeLeftCursor { get; }

		[Static]
		[Export ("resizeRightCursor")]
		NSCursor ResizeRightCursor { get; }

		[Static]
		[Export ("resizeLeftRightCursor")]
		NSCursor ResizeLeftRightCursor { get; }

		[Static]
		[Export ("resizeUpCursor")]
		NSCursor ResizeUpCursor { get; }

		[Static]
		[Export ("resizeDownCursor")]
		NSCursor ResizeDownCursor { get; }

		[Static]
		[Export ("resizeUpDownCursor")]
		NSCursor ResizeUpDownCursor { get; }

		[Static]
		[Export ("crosshairCursor")]
		NSCursor CrosshairCursor { get; }

		[Static]
		[Export ("disappearingItemCursor")]
		NSCursor DisappearingItemCursor { get; }

		[Static]
		[Export ("operationNotAllowedCursor")]
		NSCursor OperationNotAllowedCursor { get; }

		[Static]
		[Export ("dragLinkCursor")]
		NSCursor DragLinkCursor { get; }

		[Static]
		[Export ("dragCopyCursor")]
		NSCursor DragCopyCursor { get; }

		[Static]
		[Export ("contextualMenuCursor")]
		NSCursor ContextualMenuCursor { get; }

		[Lion]
		[Static]
		[Export ("IBeamCursorForVerticalLayout")]
		NSCursor IBeamCursorForVerticalLayout { get; }
		
		[Export ("initWithImage:hotSpot:")]
		IntPtr Constructor (NSImage newImage, CGPoint aPoint);

		[Export ("initWithImage:foregroundColorHint:backgroundColorHint:hotSpot:")]
		IntPtr Constructor (NSImage newImage, NSColor fg, NSColor bg, CGPoint hotSpot);

		[Static]
		[Export ("hide")]
		void Hide ();

		[Static]
		[Export ("unhide")]
		void Unhide ();

		[Static]
		[Export ("setHiddenUntilMouseMoves:")]
		void SetHiddenUntilMouseMoves (bool flag);

		//[Static]
		//[Export ("pop")]
		//void Pop ();

		[Export ("image")]
		NSImage Image { get; }

		[Export ("hotSpot")]
		CGPoint HotSpot { get; }

		[Export ("push")]
		void Push ();

		[Export ("pop")]
		void Pop ();

		[Export ("set")]
		void Set ();

		[Export ("setOnMouseExited:")]
		void SetOnMouseExited (bool flag);

		[Export ("setOnMouseEntered:")]
		void SetOnMouseEntered (bool flag);

		[Export ("isSetOnMouseExited")]
		bool IsSetOnMouseExited ();

		[Export ("isSetOnMouseEntered")]
		bool IsSetOnMouseEntered ();

		[Export ("mouseEntered:")]
		void MouseEntered (NSEvent theEvent);

		[Export ("mouseExited:")]
		void MouseExited (NSEvent theEvent);
	}

	public delegate bool NSCustomImageRepDrawingHandler(CGRect dstRect);

	[BaseType (typeof (NSImageRep))]
	[DisableDefaultCtor] // An uncaught exception was raised: -[NSCustomImageRep init]: unrecognized selector sent to instance 0x54a870
	public partial interface NSCustomImageRep {
		[Export ("initWithDrawSelector:delegate:")]
		IntPtr Constructor (Selector drawSelectorMethod, NSObject delegateObject);

		[Export ("drawSelector")]
		Selector DrawSelector { get; }
		
		[Export ("delegate", ArgumentSemantic.Assign)]  
		NSObject Delegate { get; }

		[Export("initWithSize:flipped:drawingHandler:")]
		IntPtr Constructor(CGSize size, bool flipped, NSCustomImageRepDrawingHandler drawingHandler);

		[Export("drawingHandler")]
		NSCustomImageRepDrawingHandler DrawingHandler { get; }
	}

	[BaseType (typeof (NSControl), Delegates=new string [] {"WeakDelegate"}, Events=new Type [] {typeof (NSDatePickerCellDelegate)})]
	public interface NSDatePicker {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		//Detected properties
		[Export ("datePickerStyle")]
		NSDatePickerStyle DatePickerStyle { get; set; }

		[Export ("bezeled")]
		bool Bezeled { [Bind ("isBezeled")]get; set; }

		[Export ("bordered")]
		bool Bordered { [Bind ("isBordered")]get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("cell")]
		NSDatePickerCell Cell { get; set; }

		[Export ("textColor")]
		NSColor TextColor { get; set; }

		[Export ("datePickerMode")]
		NSDatePickerMode DatePickerMode { get; set; }

		[Export ("datePickerElements")]
		NSDatePickerElementFlags DatePickerElements { get; set; }

		[Export ("calendar")]
		NSCalendar Calendar { get; set; }

		[Export ("locale")]
		NSLocale Locale { get; set; }

		[Export ("timeZone")]
		NSTimeZone TimeZone { get; set; }

		[Export ("dateValue")]
		NSDate DateValue { get; set; }

		[Export ("timeInterval")]
		double TimeInterval { get; set; }

		[Export ("minDate")]
		NSDate MinDate { get; set; }

		[Export ("maxDate")]
		NSDate MaxDate { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSDatePickerCellDelegate Delegate { get; set; }
	}

	[BaseType (typeof (NSActionCell), Delegates=new string [] {"WeakDelegate"}, Events=new Type [] {typeof (NSDatePickerCellDelegate)})]
	public interface NSDatePickerCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		//Detected properties
		[Export ("datePickerStyle")]
		NSDatePickerStyle DatePickerStyle { get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("textColor")]
		NSColor TextColor { get; set; }

		[Export ("datePickerMode")]
		NSDatePickerMode DatePickerMode { get; set; }

		[Export ("datePickerElements")]
		NSDatePickerElementFlags DatePickerElements { get; set; }

		[Export ("calendar")]
		NSCalendar Calendar { get; set; }

		[Export ("locale")]
		NSLocale Locale { get; set; }

		[Export ("timeZone")]
		NSTimeZone TimeZone { get; set; }

		[Export ("dateValue")]
		NSDate DateValue { get; set; }

		[Export ("timeInterval")]
		double TimeInterval { get; set; }

		[Export ("minDate")]
		NSDate MinDate { get; set; }

		[Export ("maxDate")]
		NSDate MaxDate { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSDatePickerCellDelegate Delegate { get; set; }

	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSDatePickerCellDelegate {
		[Export ("datePickerCell:validateProposedDateValue:timeInterval:"), EventArgs ("NSDatePickerValidator")]
		void ValidateProposedDateValue (NSDatePickerCell aDatePickerCell, ref NSDate proposedDateValue, double proposedTimeInterval);
	}

	[BaseType (typeof (NSObject))]
	public interface NSDockTile {
		[Export ("size")]
		CGSize Size { get; }

		[Export ("display")]
		void Display ();

		[Export ("owner")]
		NSObject Owner { get; }

		//Detected properties
		[Export ("contentView")]
		NSView ContentView { get; set; }

		[Export ("showsApplicationBadge")]
		bool ShowsApplicationBadge { get; set; }

		[Export ("badgeLabel"), NullAllowed]
		string BadgeLabel { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSDockTilePlugIn {
		[Abstract]
		[Export ("setDockTile:")]
		void SetDockTile (NSDockTile dockTile);

		[Abstract]
		[Export ("dockMenu")]
		NSMenu DockMenu ();
	}

	public delegate void NSDocumentCompletionHandler (IntPtr nsErrorPointerOrZero);
	
	[BaseType (typeof (NSObject))]
	public partial interface NSDocument {
		[Export ("initWithType:error:")]
		IntPtr Constructor (string typeName, out NSError outError);

		[Static]
		[Export ("canConcurrentlyReadDocumentsOfType:")]
		bool CanConcurrentlyReadDocumentsOfType (string typeName);

		[Export ("initWithContentsOfURL:ofType:error:")]
		IntPtr Constructor (NSUrl url, string typeName, out NSError outError);

		[Export ("initForURL:withContentsOfURL:ofType:error:")]
		IntPtr Constructor ([NullAllowed] NSUrl documentUrl, NSUrl documentContentsUrl, string typeName, out NSError outError);

		 [Export ("revertDocumentToSaved:")]
		 void RevertDocumentToSaved (NSObject sender);

		 [Export ("revertToContentsOfURL:ofType:error:")]
		 bool RevertToContentsOfUrl (NSUrl url, string typeName, out NSError outError);

		[Export ("readFromURL:ofType:error:")]
		bool ReadFromUrl (NSUrl url, string typeName, out NSError outError);

		[Export ("readFromFileWrapper:ofType:error:")]
		bool ReadFromFileWrapper (NSFileWrapper fileWrapper, string typeName, out NSError outError);

		[Export ("readFromData:ofType:error:")]
		bool ReadFromData (NSData data, string typeName, out NSError outError);

		[Export ("writeToURL:ofType:error:")]
		bool WriteToUrl (NSUrl url, string typeName, out NSError outError);

		[Export ("fileWrapperOfType:error:")]
		NSFileWrapper GetAsFileWrapper (string typeName, out NSError outError);

		[Export ("dataOfType:error:")]
		NSData GetAsData (string typeName, out NSError outError);

		[Export ("writeSafelyToURL:ofType:forSaveOperation:error:")]
		bool WriteSafelyToUrl (NSUrl url, string typeName, NSSaveOperationType saveOperation, out NSError outError);

		[Export ("writeToURL:ofType:forSaveOperation:originalContentsURL:error:")]
		bool WriteToUrl (NSUrl url, string typeName, NSSaveOperationType saveOperation, NSUrl absoluteOriginalContentsUrl, out NSError outError);

		[Export ("fileAttributesToWriteToURL:ofType:forSaveOperation:originalContentsURL:error:")]
		NSDictionary FileAttributesToWrite (NSUrl toUrl, string typeName, NSSaveOperationType saveOperation, NSUrl absoluteOriginalContentsUrl, out NSError outError);

		[Export ("keepBackupFile")]
		bool KeepBackupFile ();

		[Export ("saveDocument:")]
		void SaveDocument (NSObject sender);

		[Export ("saveDocumentAs:")]
		void SaveDocumentAs (NSObject sender);

		[Export ("saveDocumentTo:")]
		void SaveDocumentTo (NSObject sender);

		[Export ("saveDocumentWithDelegate:didSaveSelector:contextInfo:")]
		void SaveDocument (NSObject delegateObject, Selector didSaveSelector, IntPtr contextInfo);

		[Export ("runModalSavePanelForSaveOperation:delegate:didSaveSelector:contextInfo:")]
		void RunModalSavePanelForSaveOperation (NSSaveOperationType saveOperation, NSObject delegateObject, Selector didSaveSelector, IntPtr contextInfo);

		[Export ("shouldRunSavePanelWithAccessoryView")]
		bool ShouldRunSavePanelWithAccessoryView { get; }

		[Export ("prepareSavePanel:")]
		bool PrepareSavePanel (NSSavePanel savePanel);

		[Export ("fileNameExtensionWasHiddenInLastRunSavePanel")]
		bool FileNameExtensionWasHiddenInLastRunSavePanel { get; }

		[Export ("fileTypeFromLastRunSavePanel")]
		string FileTypeFromLastRunSavePanel { get; }

		[Export ("saveToURL:ofType:forSaveOperation:delegate:didSaveSelector:contextInfo:")]
		void SaveToUrl (NSUrl url, string typeName, NSSaveOperationType saveOperation, NSObject delegateObject, Selector didSaveSelector, IntPtr contextInfo);

		[Export ("saveToURL:ofType:forSaveOperation:error:")]
		bool SaveToUrl (NSUrl url, string typeName, NSSaveOperationType saveOperation, out NSError outError);

		[Export ("hasUnautosavedChanges")]
		bool HasUnautosavedChanges { get; }

		[Export ("autosaveDocumentWithDelegate:didAutosaveSelector:contextInfo:")]
		void AutosaveDocument (NSObject delegateObject, Selector didAutosaveSelector, IntPtr contextInfo);

		[Export ("autosavingFileType")]
		string AutosavingFileType { get; }

		[Export ("canCloseDocumentWithDelegate:shouldCloseSelector:contextInfo:")]
		void CanCloseDocument (NSObject delegateObject, Selector shouldCloseSelector, IntPtr contextInfo);

		[Export ("close")]
		void Close ();

		[Export ("runPageLayout:")]
		void RunPageLayout (NSObject sender);

		[Export ("runModalPageLayoutWithPrintInfo:delegate:didRunSelector:contextInfo:")]
		void RunModalPageLayout (NSPrintInfo printInfo, NSObject delegateObject, Selector didRunSelector, IntPtr contextInfo);

		[Export ("preparePageLayout:")]
		bool PreparePageLayout (NSPageLayout pageLayout);

		[Export ("shouldChangePrintInfo:")]
		bool ShouldChangePrintInfo (NSPrintInfo newPrintInfo);

		[Export ("printDocument:")]
		void PrintDocument (NSObject sender);

		[Export ("printDocumentWithSettings:showPrintPanel:delegate:didPrintSelector:contextInfo:")]
		void PrintDocument (NSDictionary printSettings, bool showPrintPanel, NSObject delegateObject, Selector didPrintSelector, IntPtr contextInfo);

		[Export ("printOperationWithSettings:error:")]
		NSPrintOperation PrintOperation (NSDictionary printSettings, out NSError outError);

		[Export ("runModalPrintOperation:delegate:didRunSelector:contextInfo:")]
		void RunModalPrintOperation (NSPrintOperation printOperation, NSObject delegateObject, Selector didRunSelector, IntPtr contextInfo);

		[Export ("isDocumentEdited")]
		bool IsDocumentEdited { get; }

		[Export ("updateChangeCount:")]
		void UpdateChangeCount (NSDocumentChangeType change);

		[Export ("presentError:modalForWindow:delegate:didPresentSelector:contextInfo:")]
		void PresentError (NSError error, NSWindow window, NSObject delegateObject, Selector didPresentSelector, IntPtr contextInfo);

		[Export ("presentError:")]
		bool PresentError (NSError error);

		[Export ("willPresentError:")]
		NSError WillPresentError (NSError error);

		[Export ("makeWindowControllers")]
		void MakeWindowControllers ();

		[Export ("windowNibName")]
		string WindowNibName { get; }

		[Export ("windowControllerWillLoadNib:")]
		void WindowControllerWillLoadNib (NSWindowController windowController);

		[Export ("windowControllerDidLoadNib:")]
		void WindowControllerDidLoadNib (NSWindowController windowController);

		[Export ("setWindow:")]
		void SetWindow (NSWindow window);

		[Export ("addWindowController:")]
		[PostGet ("WindowControllers")]
		void AddWindowController (NSWindowController windowController);

		[Export ("removeWindowController:")]
		[PostGet ("WindowControllers")]
		void RemoveWindowController (NSWindowController windowController);

		[Export ("showWindows")]
		void ShowWindows ();

		[Export ("windowControllers")]
		NSWindowController [] WindowControllers { get; }

		[Export ("shouldCloseWindowController:delegate:shouldCloseSelector:contextInfo:")]
		void ShouldCloseWindowController (NSWindowController windowController, NSObject delegateObject, Selector shouldCloseSelector, IntPtr contextInfo);

		[Export ("displayName")]
		string DisplayName { get; [Lion][NullAllowed] set; }

		[Export ("windowForSheet")]
		NSWindow WindowForSheet { get; }

		[Static, Export ("readableTypes")]
		string [] ReadableTypes { get; }

		[Static]
		[Export ("writableTypes")]
		string [] WritableTypes ();

		[Static]
		[Export ("isNativeType:")]
		bool IsNativeType (string type);

		[Export ("writableTypesForSaveOperation:")]
		string [] WritableTypesForSaveOperation (NSSaveOperationType saveOperation);

		[Export ("fileNameExtensionForType:saveOperation:")]
		string FileNameExtensionForSaveOperation (string typeName, NSSaveOperationType saveOperation);

		[Export ("validateUserInterfaceItem:")]
		bool ValidateUserInterfaceItem (NSObject /* Must implement NSValidatedUserInterfaceItem */ anItem);

		//Detected properties
		[Export ("fileType")]
		string FileType { get; set; }

		[Export ("fileURL"), NullAllowed]
		NSUrl FileUrl { get; set; }

		[Export ("fileModificationDate")]
		NSDate FileModificationDate { get; set; }

		[Export ("autosavedContentsFileURL")]
		NSUrl AutosavedContentsFileUrl { get; set; }

		[Export ("printInfo")]
		NSPrintInfo PrintInfo { get; set; }

		[Export ("undoManager")]
		NSUndoManager UndoManager { get; set; }

		[Export ("hasUndoManager")]
		bool HasUndoManager { get; set; }

		[Lion, Export ("performActivityWithSynchronousWaiting:usingBlock:")]
		void PerformActivity (bool waitSynchronously, Action activityCompletionHandler);

		[Lion, Export ("continueActivityUsingBlock:")]
		void ContinueActivity (Action resume);

		[Lion, Export ("continueAsynchronousWorkOnMainThreadUsingBlock:")]
		void ContinueAsynchronousWorkOnMainThread (Action work);

		[Lion, Export ("performSynchronousFileAccessUsingBlock:")]
		void PerformSynchronousFileAccess (Action fileAccessCallback);

		[Lion, Export ("performAsynchronousFileAccessUsingBlock:")]
		void PerformAsynchronousFileAccess (Action ioCode);

		[Lion, Export ("isEntireFileLoaded")]
		bool IsEntireFileLoaded { get; }

		[Lion, Export ("unblockUserInteraction")]
		void UnblockUserInteraction ();

		[Lion, Export ("autosavingIsImplicitlyCancellable")]
		bool AutosavingIsImplicitlyCancellable { get; }

		[Lion, Export ("saveToURL:ofType:forSaveOperation:completionHandler:")]
		void SaveTo (NSUrl url, string typeName, NSSaveOperationType saveOperation, NSDocumentCompletionHandler completionHandler);

		[Lion, Export ("canAsynchronouslyWriteToURL:ofType:forSaveOperation:")]
		bool CanWriteAsynchronously (NSUrl toUrl, string typeName, NSSaveOperationType saveOperation);

		[Lion, Export ("checkAutosavingSafetyAndReturnError:")]
		bool CheckAutosavingSafety (out NSError outError);

		[Lion, Export ("scheduleAutosaving")]
		void ScheduleAutosaving ();

		[Lion, Export ("autosaveWithImplicitCancellability:completionHandler:")]
		void Autosave (bool autosavingIsImplicitlyCancellable, NSDocumentCompletionHandler completionHandler);

		[Static]
		[Lion, Export ("autosavesInPlace")]
		bool AutosavesInPlace ();

		[Static]
		[Lion, Export ("preservesVersions")]
		bool PreservesVersions ();

		[Lion, Export ("duplicateDocument:")]
		void DuplicateDocument (NSObject sender);

		[Lion, Export ("duplicateDocumentWithDelegate:didDuplicateSelector:contextInfo:"), Internal]
		void _DuplicateDocument ([NullAllowed] NSObject cbackobject, [NullAllowed] Selector didDuplicateSelector, IntPtr contextInfo);

		[Lion, Export ("duplicateAndReturnError:")]
		NSDocument Duplicate (out NSError outError);

		[Lion, Export ("isInViewingMode")]
		bool IsInViewingMode { get; }

		[Lion, Export ("changeCountTokenForSaveOperation:")]
		NSObject ChangeCountToken (NSSaveOperationType saveOperation);

		[Lion, Export ("updateChangeCountWithToken:forSaveOperation:")]
		void UpdateChangeCount (NSObject changeCountToken, NSSaveOperationType saveOperation);

		[Lion, Export ("willNotPresentError:")]
		void WillNotPresentError (NSError error);

		[Lion, Export ("setDisplayName:")]
		void SetDisplayName ([NullAllowed] string displayNameOrNull);

		[Lion, Export ("restoreDocumentWindowWithIdentifier:state:completionHandler:")]
		void RestoreDocumentWindow (string identifier, NSCoder state, NSWindowCompletionHandler completionHandler);

		[Lion, Export ("encodeRestorableStateWithCoder:")]
		void EncodeRestorableState (NSCoder coder);

		[Export ("restoreStateWithCoder:")]
		void RestoreState (NSCoder coder);

		[Export ("invalidateRestorableState")]
		void InvalidateRestorableState ();

		[Static]
		[Export ("restorableStateKeyPaths")]
		string [] RestorableStateKeyPaths ();
	}

	public delegate void OpenDocumentCompletionHandler (NSDocument document, bool documentWasAlreadyOpen, NSError error);

	[BaseType (typeof (NSObject))]
	public partial interface NSDocumentController : NSWindowRestoration {
		[Static, Export ("sharedDocumentController")]
		NSObject SharedDocumentController { get; }

		[Export ("documents")]
		NSDocument [] Documents { get; }

		[Export ("currentDocument")]
		NSDocument CurrentDocument { get; }

		[Export ("currentDirectory")]
		string CurrentDirectory { get; }

		[Export ("documentForURL:")]
		NSDocument DocumentForUrl (NSUrl url);

		[Export ("documentForWindow:")]
		NSDocument DocumentForWindow (NSWindow window);

		[Export ("addDocument:")]
		[PostGet ("Documents")]
		void AddDocument (NSDocument document);

		[Export ("removeDocument:")]
		[PostGet ("Documents")]
		void RemoveDocument (NSDocument document);

		[Export ("newDocument:")]
		void NewDocument ([NullAllowed] NSObject sender);

		[Export ("openUntitledDocumentAndDisplay:error:")]
		NSObject OpenUntitledDocument (bool displayDocument, out NSError outError);

		[Export ("makeUntitledDocumentOfType:error:")]
		NSObject MakeUntitledDocument (string typeName, out NSError error);

		[Export ("openDocument:")]
		void OpenDocument ([NullAllowed] NSObject sender);

		[Export ("URLsFromRunningOpenPanel")]
		NSUrl [] UrlsFromRunningOpenPanel ();

		[Export ("runModalOpenPanel:forTypes:")]
		nint RunModalOpenPanel (NSOpenPanel openPanel, string [] types);

		[Export ("openDocumentWithContentsOfURL:display:error:")]
		NSObject OpenDocument (NSUrl url, bool displayDocument, out NSError outError);

		[Lion]
		[Export ("openDocumentWithContentsOfURL:display:completionHandler:")]
		void OpenDocument (NSUrl url, bool display, OpenDocumentCompletionHandler completionHandler);

		[Export ("makeDocumentWithContentsOfURL:ofType:error:")]
		NSObject MakeDocument (NSUrl url, string typeName, out NSError outError);

		[Export ("reopenDocumentForURL:withContentsOfURL:error:")]
		bool ReopenDocument (NSUrl url, NSUrl contentsUrl, out NSError outError);

		[Export ("makeDocumentForURL:withContentsOfURL:ofType:error:")]
		NSObject MakeDocument ([NullAllowed] NSUrl urlOrNil, NSUrl contentsUrl, string typeName, out NSError outError);

		[Export ("saveAllDocuments:")]
		void SaveAllDocuments ([NullAllowed] NSObject sender);

		[Export ("hasEditedDocuments")]
		bool HasEditedDocuments { get; }

		[Export ("reviewUnsavedDocumentsWithAlertTitle:cancellable:delegate:didReviewAllSelector:contextInfo:")]
		void ReviewUnsavedDocuments (string title, bool cancellable, NSObject delegateObject, Selector didReviewAllSelector, IntPtr contextInfo);

		[Export ("closeAllDocumentsWithDelegate:didCloseAllSelector:contextInfo:")]
		void CloseAllDocuments (NSObject delegateObject, Selector didCloseAllSelector, IntPtr contextInfo);

		[Export ("presentError:modalForWindow:delegate:didPresentSelector:contextInfo:")]
		void PresentError (NSError error, NSWindow window, NSObject delegateObject, Selector didPresentSelector, IntPtr contextInfo);

		[Export ("presentError:")]
		bool PresentError (NSError error);

		[Export ("willPresentError:")]
		NSError WillPresentError (NSError error);

		[Export ("maximumRecentDocumentCount")]
		nint MaximumRecentDocumentCount { get; }

		[Export ("clearRecentDocuments:")]
		void ClearRecentDocuments ([NullAllowed] NSObject sender);

		[Export ("noteNewRecentDocument:")]
		void NoteNewRecentDocument (NSDocument document);

		[Export ("noteNewRecentDocumentURL:")]
		void NoteNewRecentDocumentURL (NSUrl url);

		[Export ("recentDocumentURLs")]
		NSUrl [] RecentDocumentUrls { get; }

		[Export ("defaultType")]
		string DefaultType { get; }

		[Export ("typeForContentsOfURL:error:")]
		string TypeForUrl (NSUrl url, out NSError outError);

		[Export ("documentClassNames")]
		string [] DocumentClassNames  {get; }

		[Export ("documentClassForType:")]
		Class DocumentClassForType (string typeName);

		[Export ("displayNameForType:")]
		string DisplayNameForType (string typeName);

		[Export ("validateUserInterfaceItem:")]
		bool ValidateUserInterfaceItem (NSObject /* must implement NSValidatedUserInterfaceItem */ anItem);

		//Detected properties
		[Export ("autosavingDelay")]
		double AutosavingDelay { get; set; }
	}

	[Lion]
	[BaseType (typeof (NSObject))]
	public interface NSDraggingImageComponent {
		[Export ("key")]
		string Key { get; set;  }

		[Export ("contents")]
		NSObject Contents { get; set;  }

		[Export ("frame")]
		CGRect Frame { get; set;  }

		[Static]
		[Export ("draggingImageComponentWithKey:")]
		NSDraggingImageComponent FromKey (string key);

		[Export ("initWithKey:")]
		IntPtr Constructor (string key);

		[Field ("NSDraggingImageComponentIconKey")]
		NSString IconKey { get; }

		[Field ("NSDraggingImageComponentLabelKey")]
		NSString LabelKey { get; }
	}

	public delegate NSDraggingImageComponent [] NSDraggingItemImagesContentProvider ();
	
	[BaseType (typeof (NSObject))]
	public interface NSDraggingItem {
		[Export ("item")]
		NSObject Item { get;  }

		[Export ("draggingFrame")]
		CGRect DraggingFrame { get; set;  }

		[Export ("imageComponents")]
		NSDraggingImageComponent [] ImageComponents { get;  }

		[Export ("initWithPasteboardWriter:")]
		IntPtr Constructor (NSPasteboardWriting pasteboardWriter);

		[Export ("setImageComponentsProvider:")]
		void SetImagesContentProvider ([NullAllowed] NSDraggingItemImagesContentProvider provider);

		[Export ("setDraggingFrame:contents:")]
		void SetDraggingFrame (CGRect frame, NSObject contents);

	}
	
	//NSDraggingInfo is documented as a protocol, but it doesn't work as a protocol.
	//per the docs: "In Java, sender is an NSDragDestination object, which implements the NSDraggingInfo interface." - from Drag and Drop Programming Topics for Cocoa
	//furthermore, "you never need to create a class that implements the NSDraggingInfo protocol" from NSDraggingInfo Protocol Reference
	[BaseType (typeof (NSObject), Name="NSDragDestination")]
	public interface NSDraggingInfo  {
		[Export ("draggingDestinationWindow")]
		NSWindow DraggingDestinationWindow { get; }

		[Export ("draggingSourceOperationMask")]
		NSDragOperation DraggingSourceOperationMask { get; }

		[Export ("draggingLocation")]
		CGPoint DraggingLocation { get; }
	
		[Export ("draggedImageLocation")]
		CGPoint DraggedImageLocation { get; }

		[Export ("draggedImage")]
		NSImage DraggedImage { get; }

		[Export ("draggingPasteboard")]
		NSPasteboard DraggingPasteboard { get; }

		[Export ("draggingSource")]
		NSObject DraggingSource { get; }

		[Export ("draggingSequenceNumber")]
		nint DraggingSequenceNumber { get; }

		[Export ("slideDraggedImageTo:")]
		void SlideDraggedImageTo (CGPoint screenPoint);

		[Export ("namesOfPromisedFilesDroppedAtDestination:")]
		string [] PromisedFilesDroppedAtDestination (NSUrl dropDestination);

		[Lion]
		[Export ("animatesToDestination")]
		bool AnimatesToDestination { get; set; }

		[Lion]
		[Export ("numberOfValidItemsForDrop")]
		nint NumberOfValidItemsForDrop { get; set; }

		[Lion]
		[Export ("draggingFormation")]
		NSDraggingFormation DraggingFormation { get; set; } 

		[Lion]
		[Export ("enumerateDraggingItemsWithOptions:forView:classes:searchOptions:usingBlock:")]
		void EnumerateDraggingItems (NSDraggingItemEnumerationOptions enumOpts, NSView view, NSPasteboardReading [] classArray,
					     NSDictionary searchOptions, NSDraggingEnumerator enumerator);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSDraggingDestination {
		[Export ("draggingEntered:"), DefaultValue (NSDragOperation.None)]
		NSDragOperation DraggingEntered (NSDraggingInfo sender);

		[Export ("draggingUpdated:"), DefaultValue (NSDragOperation.None)]
		NSDragOperation DraggingUpdated (NSDraggingInfo sender);

		[Export ("draggingExited:")]
		void DraggingExited (NSDraggingInfo sender);

		[Export ("prepareForDragOperation:"), DefaultValue (false)]
		bool PrepareForDragOperation (NSDraggingInfo sender);

		[Export ("performDragOperation:"), DefaultValue (false)]
		bool PerformDragOperation (NSDraggingInfo sender);

		[Export ("concludeDragOperation:")]
		void ConcludeDragOperation (NSDraggingInfo sender);

		[Export ("draggingEnded:")]
		void DraggingEnded (NSDraggingInfo sender);

		[Export ("wantsPeriodicDraggingUpdates"), DefaultValue (true)]
		bool WantsPeriodicDraggingUpdates { get; }
	}

	public delegate void NSDraggingEnumerator (NSDraggingItem draggingItem, int idx, ref bool stop);
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // warning on dispose - created using NSView.BeginDraggingSession
	public interface NSDraggingSession {
		[Export ("draggingFormation")]
		NSDraggingFormation DraggingFormation { get; set;  }

		[Export ("animatesToStartingPositionsOnCancelOrFail")]
		bool AnimatesToStartingPositionsOnCancelOrFail { get; set;  }

		[Export ("draggingLeaderIndex")]
		nint DraggingLeaderIndex { get; set;  }

		[Export ("draggingPasteboard")]
		NSPasteboard DraggingPasteboard { get;  }

		[Export ("draggingSequenceNumber")]
		nint DraggingSequenceNumber { get;  }

		[Export ("draggingLocation")]
		CGPoint DraggingLocation { get;  }

		[Export ("enumerateDraggingItemsWithOptions:forView:classes:searchOptions:usingBlock:")]
		void EnumerateDraggingItems (NSDraggingItemEnumerationOptions enumOpts, NSView view, Class [] classArray, NSDictionary searchOptions, NSDraggingEnumerator enumerator);

	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSDraggingSource {
		[Export ("draggingSourceOperationMaskForLocal:"), DefaultValue (NSDragOperation.None)]
		NSDragOperation DraggingSourceOperationMaskForLocal (bool flag);

		[Export ("namesOfPromisedFilesDroppedAtDestination:"), DefaultValue (new string[0])]
		string [] NamesOfPromisedFilesDroppedAtDestination (NSUrl dropDestination);

		[Export ("draggedImage:beganAt:")]
		void DraggedImageBeganAt (NSImage image, CGPoint screenPoint);

		[Export ("draggedImage:endedAt:operation:")]
		void DraggedImageEndedAtOperation (NSImage image, CGPoint screenPoint, NSDragOperation operation);

		[Export ("draggedImage:movedTo:")]
		void DraggedImageMovedTo (NSImage image, CGPoint screenPoint);

		[Export ("ignoreModifierKeysWhileDragging"), DefaultValue (false)]
		bool IgnoreModifierKeysWhileDragging { get; }

		[Deprecated (PlatformName.MacOSX, 10, 1, message: "use DraggedImageEndedAtOperation() instead")]
		[Export ("draggedImage:endedAt:deposited:")]
		void DraggedImageEndedAtDeposited (NSImage image, CGPoint screenPoint, bool deposited);
	}
	
	[BaseType (typeof (NSResponder), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSDrawerDelegate)})]
	public partial interface NSDrawer {
		[Export ("initWithContentSize:preferredEdge:")]
		IntPtr Constructor (CGSize contentSize, NSRectEdge edge);

		[Export ("parentWindow")]
		NSWindow ParentWindow { get; set; }

		[Export ("contentView")]
		NSView ContentView { get; set; }

		[Export ("preferredEdge")]
		NSRectEdge PreferredEdge { get; set; }
		
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSDrawerDelegate Delegate { get; set; }

		//[Export ("open")]
		//void Open ();

		[Export ("openOnEdge:")]
		void OpenOnEdge (NSRectEdge edge);

		//[Export ("close")]
		//void Close ();

		[Export ("open:")]
		void Open (NSObject sender);

		[Export ("close:")]
		void Close (NSObject sender);

		[Export ("toggle:")]
		void Toggle (NSObject sender);

		[Export ("state")]
		NSDrawerState State { get; }

		[Export ("edge")]
		NSRectEdge Edge { get; }

		[Export ("contentSize")]
		CGSize ContentSize { get; set; }

		[Export ("minContentSize")]
		CGSize MinContentSize { get; set; }

		[Export ("maxContentSize")]
		CGSize MaxContentSize { get; set; }

		[Export ("leadingOffset")]
		nfloat LeadingOffset { get; set; }

		[Export ("trailingOffset")]
		nfloat TrailingOffset { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSDrawerDelegate {
		[Export ("drawerDidClose:"), EventArgs ("NSNotification")]
		void DrawerDidClose (NSNotification notification);
		
		[Export ("drawerDidOpen:"), EventArgs ("NSNotification")]
		void DrawerDidOpen (NSNotification notification);

		[Export ("drawerShouldClose:"), DelegateName ("DrawerShouldCloseDelegate"), DefaultValue (true)]
		bool DrawerShouldClose (NSDrawer sender);

		[Export ("drawerShouldOpen:"), DelegateName ("DrawerShouldOpenDelegate"), DefaultValue (true)]
		bool DrawerShouldOpen (NSDrawer sender);
	
		[Export ("drawerWillClose:"), EventArgs ("NSNotification")]
		void DrawerWillClose (NSNotification notification);
	
		[Export ("drawerWillOpen:"), EventArgs ("NSNotification")]
		void DrawerWillOpen (NSNotification notification);

		[Export ("drawerWillResizeContents:toSize:"), DelegateName ("DrawerWillResizeContentsDelegate"), DefaultValue (null)]
		CGSize DrawerWillResizeContents (NSDrawer sender, CGSize toSize);

	}

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // crash at runtime (e.g. description). Documentation state: "You don’t create NSFont objects using the alloc and init methods."
	public partial interface NSFont {
		[Static]
		[Export ("fontWithName:size:")]
		NSFont FromFontName (string fontName, nfloat fontSize);

		//[Static]
		//[Export ("fontWithName:matrix:")]
		//NSFont FromFontName (string fontName, nfloat [] fontMatrix);

		[Static]
		[Export ("fontWithDescriptor:size:")]
		NSFont FromDescription (NSFontDescriptor fontDescriptor, nfloat fontSize);

		[Static]
		[Export ("fontWithDescriptor:textTransform:")]
		NSFont FromDescription (NSFontDescriptor fontDescriptor, [NullAllowed] NSAffineTransform textTransform);

		[Static]
		[Export ("userFontOfSize:")]
		NSFont UserFontOfSize (nfloat fontSize);

		[Static]
		[Export ("userFixedPitchFontOfSize:")]
		NSFont UserFixedPitchFontOfSize (nfloat fontSize);

		[Static]
		[Export ("setUserFont:")]
		void SetUserFont (NSFont aFont);

		[Static]
		[Export ("setUserFixedPitchFont:")]
		void SetUserFixedPitchFont (NSFont aFont);

		[Static]
		[Export ("systemFontOfSize:")]
		NSFont SystemFontOfSize (nfloat fontSize);

		[Static]
		[Export ("boldSystemFontOfSize:")]
		NSFont BoldSystemFontOfSize (nfloat fontSize);

		[Static]
		[Export ("labelFontOfSize:")]
		NSFont LabelFontOfSize (nfloat fontSize);

		[Static]
		[Export ("titleBarFontOfSize:")]
		NSFont TitleBarFontOfSize (nfloat fontSize);

		[Static]
		[Export ("menuFontOfSize:")]
		NSFont MenuFontOfSize (nfloat fontSize);

		[Static]
		[Export("menuBarFontOfSize:")]
		NSFont MenuBarFontOfSize (nfloat fontSize);

		[Static]
		[Export("messageFontOfSize:")]
		NSFont MessageFontOfSize (nfloat fontSize);

		[Static]
		[Export ("paletteFontOfSize:")]
		NSFont PaletteFontOfSize (nfloat fontSize);

		[Static]
		[Export ("toolTipsFontOfSize:")]
		NSFont ToolTipsFontOfSize (nfloat fontSize);

		[Static]
		[Export ("controlContentFontOfSize:")]
		NSFont ControlContentFontOfSize (nfloat fontSize);

		[Static]
		[Export ("systemFontSize")]
		nfloat SystemFontSize { get; }

		[Static]
		[Export ("smallSystemFontSize")]
		nfloat SmallSystemFontSize { get; }

		[Static]
		[Export ("labelFontSize")]
		nfloat LabelFontSize { get; }

		[Static]
		[Export ("systemFontSizeForControlSize:")]
		nfloat SystemFontSizeForControlSize (NSControlSize controlSize);

		[Export ("fontName")]
		string FontName { get; }

		[Export ("pointSize")]
		nfloat PointSize { get; }

		//[Export ("matrix")]
		//  FIXME
		//IntPtr *float Matrix { get; }

		[Export ("familyName")]
		string FamilyName { get; }

		[Export ("displayName")]
		string DisplayName { get; }

		[Export ("fontDescriptor")]
		NSFontDescriptor FontDescriptor { get; }

		[Export ("textTransform")]
		NSAffineTransform TextTransform { get; }

		[Export ("numberOfGlyphs")]
		nuint GlyphCount { get; }

		[Export ("mostCompatibleStringEncoding")]
		NSStringEncoding MostCompatibleStringEncoding { get; }

		[Export ("glyphWithName:")]
		uint GlyphWithName (string aName);

		[Export ("coveredCharacterSet")]
		NSCharacterSet CoveredCharacterSet { get; }

		[Export ("boundingRectForFont")]
		CGRect BoundingRectForFont { get; }

		[Export ("maximumAdvancement")]
		CGSize MaximumAdvancement { get; }

		[Export ("ascender")]
		nfloat Ascender { get; }

		[Export ("descender")]
		nfloat Descender { get; }

		[Export ("leading")]
		nfloat Leading { get; }

		[Export ("underlinePosition")]
		nfloat UnderlinePosition { get; }

		[Export ("underlineThickness")]
		nfloat UnderlineThickness { get; }

		[Export ("italicAngle")]
		nfloat ItalicAngle { get; }

		[Export ("capHeight")]
		nfloat CapHeight { get; }

		[Export ("xHeight")]
		nfloat XHeight { get; }

		[Export ("isFixedPitch")]
		bool IsFixedPitch { get; }

		[Export ("boundingRectForGlyph:")]
		CGRect BoundingRectForGlyph (uint aGlyph);

		[Export ("advancementForGlyph:")]
		CGSize AdvancementForGlyph (uint ag);

		// FIXME binding
		//[Export ("getBoundingRects:forGlyphs:count:")]
		//void GetBoundingRectsforGlyphscount (CGRect *bounds, uint glyphs, int glyphCount);

		// FIXME binding
		//[Export ("getAdvancements:forGlyphs:count:")]
		//void GetAdvancementsforGlyphscount (CGSizeArray advancements, const uint glyphs, int glyphCount);

		// FIXME binding
		//[Export ("getAdvancements:forPackedGlyphs:length:")]
		//void GetAdvancementsforPackedGlyphslength (CGSizeArray advancements, void *packedGlyphs, uint length);

		[Export ("set")]
		void Set ();

		[Export ("setInContext:")]
		void SetInContext (NSGraphicsContext graphicsContext);

		[Export ("printerFont")]
		NSFont PrinterFont { get; }

		[Export ("screenFont")]
		NSFont ScreenFont { get; }

		[Export ("screenFontWithRenderingMode:")]
		NSFont ScreenFontWithRenderingMode (NSFontRenderingMode renderingMode);

		[Export ("renderingMode")]
		NSFontRenderingMode RenderingMode { get; }

		[Export ("isVertical")]
		bool IsVertical { get; }

		//
		// Not a property because this causes the creation of a new font on request in the specified configuration.
		//
		[Export ("verticalFont")]
		NSFont GetVerticalFont ();
	}

	[Lion]
	public interface NSFontCollectionChangedEventArgs {
		[Internal, Export ("NSFontCollectionActionKey")]
		NSString _Action { get; }

		[Export ("NSFontCollectionNameKey")]
		string Name { get; }

		[Export ("NSFontCollectionOldNameKey")]
		string OldName { get; }

		[Internal, Export ("NSFontCollectionVisibilityKey")]
		NSNumber _Visibility { get; }
	}

	[Lion]
	[BaseType (typeof (NSObject))]
	interface NSFontCollection {
		[Static]
		[Export ("fontCollectionWithDescriptors:")]
		NSFontCollection FromDescriptors (NSFontDescriptor [] queryDescriptors);

		[Static]
		[Export ("fontCollectionWithAllAvailableDescriptors")]
		NSFontCollection GetAllAvailableFonts ();

		[Static]
		[Export ("fontCollectionWithLocale:")]
		NSFontCollection FromLocale (NSLocale locale);

		[Static]
		[Export ("showFontCollection:withName:visibility:error:")]
		bool ShowFontCollection (NSFontCollection fontCollection, string name, NSFontCollectionVisibility visibility, out NSError error);

		[Static]
		[Export ("hideFontCollectionWithName:visibility:error:")]
		bool HideFontCollection (string name, NSFontCollectionVisibility visibility, out NSError error);

		[Static]
		[Export ("renameFontCollectionWithName:visibility:toName:error:")]
		bool RenameFontCollection (string fromName, NSFontCollectionVisibility visibility, string toName, out NSError error);

		[Static]
		[Export ("allFontCollectionNames")]
		string [] AllFontCollectionNames { get; }

		[Static]
		[Export ("fontCollectionWithName:")]
		NSFontCollection FromName (string name);

		[Static]
		[Export ("fontCollectionWithName:visibility:")]
		NSFontCollection FromName (string name, NSFontCollectionVisibility visibility);

		[Export ("queryDescriptors")]
		NSFontDescriptor [] GetQueryDescriptors ();

		[Export ("exclusionDescriptors")]
		NSFontDescriptor [] GetExclusionDescriptors ();

		[Export ("matchingDescriptors")]
		NSFontDescriptor [] GetMatchingDescriptors ();

		[Export ("matchingDescriptorsWithOptions:")]
		NSFontDescriptor [] GetMatchingDescriptors (NSDictionary options);

		[Export ("matchingDescriptorsForFamily:")]
		NSFontDescriptor [] GetMatchingDescriptors (string family);

		[Export ("matchingDescriptorsForFamily:options:")]
		NSFontDescriptor [] GetMatchingDescriptors (string family, NSDictionary options);

		[Field ("NSFontCollectionIncludeDisabledFontsOption")]
		NSString IncludeDisabledFontsOption { get; }
		
		[Field ("NSFontCollectionRemoveDuplicatesOption")]
		NSString RemoveDuplicatesOption { get; }
		
		[Field ("NSFontCollectionDisallowAutoActivationOption")]
		NSString DisallowAutoActivationOption { get; }
		
		[Notification (typeof (NSFontCollectionChangedEventArgs)), Field ("NSFontCollectionDidChangeNotification")]
		NSString ChangedNotification { get; }
		
		[Field ("NSFontCollectionActionKey")]
		NSString ActionKey { get; }
		
		[Field ("NSFontCollectionNameKey")]
		NSString NameKey { get; }
		
		[Field ("NSFontCollectionOldNameKey")]
		NSString OldNameKey { get; }
		
		[Field ("NSFontCollectionVisibilityKey")]
		NSString VisibilityKey { get; }
		
		[Field ("NSFontCollectionWasShown")]
		NSString ActionWasShown { get; }
		
		[Field ("NSFontCollectionWasHidden")]
		NSString ActionWasHidden { get; }
		
		[Field ("NSFontCollectionWasRenamed")]
		NSString ActionWasRenamed { get; }
		
		[Field ("NSFontCollectionAllFonts")]
		NSString NameAllFonts { get; }
		
		[Field ("NSFontCollectionUser")]
		NSString NameUser { get; }
		
		[Field ("NSFontCollectionFavorites")]
		NSString NameFavorites { get; }
		
		[Field ("NSFontCollectionRecentlyUsed")]
		NSString NameRecentlyUsed { get; }
		
	}

	[Lion]
	[BaseType (typeof (NSFontCollection))]
	interface NSMutableFontCollection {
		[Export ("setQueryDescriptors:")]
		void SetQueryDescriptors (NSFontDescriptor [] descriptors);

		[Export ("setExclusionDescriptors:")]
		void SetExclusionDescriptors (NSFontDescriptor [] descriptors);

		[Export ("addQueryForDescriptors:")]
		void AddQueryForDescriptors (NSFontDescriptor [] descriptors);

		[Export ("removeQueryForDescriptors:")]
		void RemoveQueryForDescriptors (NSFontDescriptor [] descriptors);

	}	

	[BaseType (typeof (NSObject))]
	public interface NSFontDescriptor {
		[Export ("postscriptName")]
		string PostscriptName { get; }

		[Export ("pointSize")]
		nfloat PointSize { get; }

		[Export ("matrix")]
		NSAffineTransform Matrix { get; }

		[Export ("symbolicTraits")]
		NSFontSymbolicTraits SymbolicTraits { get; }

		[Export ("objectForKey:")]
		NSObject ObjectForKey (string key);

		[Export ("fontAttributes")]
		NSDictionary FontAttributes { get; }

		[Static]
		[Export ("fontDescriptorWithFontAttributes:")]
		NSFontDescriptor FromAttributes (NSDictionary attributes);

		[Static]
		[Export ("fontDescriptorWithName:size:")]
		NSFontDescriptor FromNameSize (string fontName, nfloat size);

		[Static]
		[Export ("fontDescriptorWithName:matrix:")]
		NSFontDescriptor FromNameMatrix (string fontName, NSAffineTransform matrix);

		[Export ("initWithFontAttributes:")]
		IntPtr Constructor (NSDictionary attributes);

		[Export ("matchingFontDescriptorsWithMandatoryKeys:")]
		NSFontDescriptor [] MatchingFontDescriptors (NSSet mandatoryKeys);

		[Export ("matchingFontDescriptorWithMandatoryKeys:")]
		NSFontDescriptor MatchingFontDescriptorWithMandatoryKeys (NSSet mandatoryKeys);

		[Export ("fontDescriptorByAddingAttributes:")]
		NSFontDescriptor FontDescriptorByAddingAttributes (NSDictionary attributes);

		[Export ("fontDescriptorWithSymbolicTraits:")]
		NSFontDescriptor FontDescriptorWithSymbolicTraits (NSFontSymbolicTraits symbolicTraits);

		[Export ("fontDescriptorWithSize:")]
		NSFontDescriptor FontDescriptorWithSize (nfloat newPointSize);

		[Export ("fontDescriptorWithMatrix:")]
		NSFontDescriptor FontDescriptorWithMatrix (NSAffineTransform matrix);

		[Export ("fontDescriptorWithFace:")]
		NSFontDescriptor FontDescriptorWithFace (string newFace);

		[Export ("fontDescriptorWithFamily:")]
		NSFontDescriptor FontDescriptorWithFamily (string newFamily);
	}

	[BaseType (typeof (NSObject))]
	public interface NSFontManager {
		[Static, Export ("setFontPanelFactory:")]
		void SetFontPanelFactory (Class factoryId);

		[Static, Export ("setFontManagerFactory:")]
		void SetFontManagerFactory (Class factoryId);

		[Static, Export ("sharedFontManager")]
		NSFontManager SharedFontManager { get; }

		[Export ("isMultiple")]
		bool IsMultiple { get; }

		[Export ("selectedFont")]
		NSFont SelectedFont { get; }

		[Export ("setSelectedFont:isMultiple:")]
		void SetSelectedFont (NSFont fontObj, bool isMultiple);

		[Export ("setFontMenu:")]
		void SetFontMenu (NSMenu newMenu);

		[Export ("fontMenu:")]
		NSMenu FontMenu (bool create);

		[Export ("fontPanel:")]
		NSFontPanel FontPanel (bool create);

		[Export ("fontWithFamily:traits:weight:size:")]
		NSFont FontWithFamily (string family, NSFontTraitMask traits, nint weight, nfloat size);

		[Export ("traitsOfFont:")]
		NSFontTraitMask TraitsOfFont (NSFont fontObj);

		[Export ("weightOfFont:")]
		nint WeightOfFont (NSFont fontObj);

		[Export ("availableFonts")]
		string [] AvailableFonts { get; }

		[Export ("availableFontFamilies")]
		string [] AvailableFontFamilies { get; }

		[Export ("availableMembersOfFontFamily:")]
		NSArray [] AvailableMembersOfFontFamily (string fam);

		[Export ("convertFont:")]
		NSFont ConvertFont (NSFont fontObj);

		[Export ("convertFont:toSize:")]
		NSFont ConvertFont (NSFont fontObj, nfloat size);

		[Export ("convertFont:toFace:")]
		NSFont ConvertFont (NSFont fontObj, string typeface);

		[Export ("convertFont:toFamily:")]
		NSFont ConvertFontToFamily (NSFont fontObj, string family);

		[Export ("convertFont:toHaveTrait:")]
		NSFont ConvertFont (NSFont fontObj, NSFontTraitMask trait);

		[Export ("convertFont:toNotHaveTrait:")]
		NSFont ConvertFontToNotHaveTrait (NSFont fontObj, NSFontTraitMask trait);

		[Export ("convertWeight:ofFont:")]
		NSFont ConvertWeight (bool increaseWeight, NSFont fontObj);

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		[Export ("action"), NullAllowed]
		Selector Action { get; set; }

		[Export ("sendAction")]
		bool SendAction { get; }

		[Export ("delegate")][NullAllowed]
		NSObject WeakDelegate { get; set; } 

		[Export ("localizedNameForFamily:face:")]
		string LocalizedNameForFamily (string family, [NullAllowed] string faceKey);

		[Export ("setSelectedAttributes:isMultiple:")]
		void SetSelectedAttributes (NSDictionary attributes, bool isMultiple);

		[Export ("convertAttributes:")]
		NSDictionary ConvertAttributes (NSDictionary attributes);

		[Export ("availableFontNamesMatchingFontDescriptor:")]
		string [] AvailableFontNamesMatchingFontDescriptor (NSFontDescriptor descriptor);

		[Export ("collectionNames")]
		string [] CollectionNames { get; }

		[Export ("fontDescriptorsInCollection:")]
		NSArray FontDescriptorsInCollection (string collectionNames);

		[Export ("addCollection:options:")]
		bool AddCollection (string collectionName, NSFontCollectionOptions collectionOptions);

		[Export ("removeCollection:")]
		bool RemoveCollection (string collectionName);

		[Export ("addFontDescriptors:toCollection:")]
		void AddFontDescriptors (NSFontDescriptor [] descriptors, string collectionName);

		[Export ("removeFontDescriptor:fromCollection:")]
		void RemoveFontDescriptor (NSFontDescriptor descriptor, string collection);

		[Export ("currentFontAction")]
		nint CurrentFontAction { get; }

		[Export ("convertFontTraits:")]
		NSFontTraitMask ConvertFontTraits (NSFontTraitMask traits);

		[Export ("target"), NullAllowed]
		NSObject Target { get; set; }

		[Export ("fontNamed:hasTraits:")]
		bool FontNamedHasTraits (string fName, NSFontTraitMask someTraits);

		[Export ("availableFontNamesWithTraits:")]
		string [] AvailableFontNamesWithTraits (NSFontTraitMask someTraits);

		[Export ("addFontTrait:")]
		void AddFontTrait (NSObject sender);

		[Export ("removeFontTrait:")]
		void RemoveFontTrait (NSObject sender);

		[Export ("modifyFontViaPanel:")]
		void ModifyFontViaPanel (NSObject sender);

		[Export ("modifyFont:")]
		void ModifyFont (NSObject sender);

		[Export ("orderFrontFontPanel:")]
		void OrderFrontFontPanel (NSObject sender);

		[Export ("orderFrontStylesPanel:")]
		void OrderFrontStylesPanel (NSObject sender);
	}

	[BaseType (typeof (NSPanel))]
	public interface NSFontPanel {
		[Static]
		[Export ("sharedFontPanel")]
		NSFontPanel SharedFontPanel { get; }

		[Static]
		[Export ("sharedFontPanelExists")]
		bool SharedFontPanelExists { get; }

		[Export ("setPanelFont:isMultiple:")]
		void SetPanelFont (NSFont fontObj, bool isMultiple);

		[Export ("panelConvertFont:")]
		NSFont PanelConvertFont (NSFont fontObj);

		[Export ("worksWhenModal")]
		bool WorksWhenModal { get; }

		[Export ("reloadDefaultFontFamilies")]
		void ReloadDefaultFontFamilies ();

		//Detected properties
		[Export ("accessoryView"), NullAllowed]
		NSView AccessoryView { get; set; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")]get; set; }
	}
	
	[BaseType (typeof (NSMatrix))]
	public partial interface NSForm  {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("initWithFrame:mode:prototype:numberOfRows:numberOfColumns:")]
		IntPtr Constructor (CGRect frameRect, NSMatrixMode aMode, NSCell aCell, nint rowsHigh, nint colsWide);

		[Export ("initWithFrame:mode:cellClass:numberOfRows:numberOfColumns:")]
		IntPtr Constructor (CGRect frameRect, NSMatrixMode aMode, Class factoryId, nint rowsHigh, nint colsWide);

		[Export ("indexOfSelectedItem")]
		nint SelectedItemIndex { get; }

		[Export ("setEntryWidth:")]
		void SetEntryWidth (nfloat width);

		[Export ("setInterlineSpacing:")]
		void SetInterlineSpacing (nfloat spacing);

		[Export ("setBordered:")]
		void SetBordered (bool bordered);

		[Export ("setBezeled:")]
		void SetBezeled (bool bezeled);

		[Export ("setTitleAlignment:")]
		void SetTitleAlignment (NSTextAlignment mode);

		[Export ("setTextAlignment:")]
		void SetTextAlignment (NSTextAlignment mode);

		[Export ("setTitleFont:")]
		void SetTitleFont (NSFont fontObj);

		[Export ("setTextFont:")]
		void SetTextFont (NSFont fontObj);

		[Export ("cellAtIndex:")]
		NSObject CellAtIndex (nint index);

		[Export ("drawCellAtIndex:")]
		void DrawCellAtIndex (nint index);

		[Export ("addEntry:")]
		NSFormCell AddEntry (string title);

		[Export ("insertEntry:atIndex:")]
		NSFormCell InsertEntryatIndex (string title, nint index);

		[Export ("removeEntryAtIndex:")]
		void RemoveEntryAtIndex (nint index);

		[Export ("indexOfCellWithTag:")]
		nint IndexOfCellWithTag (nint aTag);

		[Export ("selectTextAtIndex:")]
		void SelectTextAtIndex (nint index);

		[Export ("setFrameSize:")]
		void SetFrameSize (CGSize newSize);

		[Export ("setTitleBaseWritingDirection:")]
		void SetTitleBaseWritingDirection (NSWritingDirection writingDirection);

		[Export ("setTextBaseWritingDirection:")]
		void SetTextBaseWritingDirection (NSWritingDirection writingDirection);
	}
	
	[BaseType (typeof (NSActionCell))]
	public partial interface NSFormCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("initTextCell:")]
		IntPtr ConstrainScrollPoint (string aString);

		[Export ("isOpaque")]
		bool IsOpaque { get; }

		//Detected properties
		[Export ("titleWidth")]
		nfloat TitleWidth { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("titleFont")]
		NSFont TitleFont { get; set; }

		[Export ("titleAlignment")]
		NSTextAlignment TitleAlignment { get; set; }

		[Export ("placeholderString")]
		string PlaceholderString { get; set; }

		[Export ("placeholderAttributedString")]
		NSAttributedString PlaceholderAttributedString { get; set; }

		[Export ("titleBaseWritingDirection")]
		NSWritingDirection TitleBaseWritingDirection { get; set; }

		[Export ("setTitleWithMnemonic:")]
		void SetTitleWithMnemonic (string  stringWithAmpersand);
		
		[Export ("attributedTitle")]
		NSAttributedString AttributedTitle { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSGlyphGenerator {
		[Export ("generateGlyphsForGlyphStorage:desiredNumberOfCharacters:glyphIndex:characterIndex:")]
		void GenerateGlyphs (NSObject nsGlyphStorageOrNSLayoutManager, nuint nchars, ref nuint glyphIndex, ref nuint charIndex);

		[Static, Export ("sharedGlyphGenerator")]
		NSGlyphGenerator SharedGlyphGenerator { get; }
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSGradient {
		[Export ("initWithStartingColor:endingColor:")]
		IntPtr Constructor  (NSColor startingColor, NSColor endingColor);

		[Export ("initWithColors:")]
		IntPtr Constructor  (NSColor[] colorArray);

		// See AppKit/NSGradiant.cs
		//[Export ("initWithColorsAndLocations:")]
		//[Export ("initWithColors:atLocations:colorSpace:")]

		[Export ("drawFromPoint:toPoint:options:")]
		void DrawFromPoint (CGPoint startingPoint, CGPoint endingPoint, NSGradientDrawingOptions options);

		[Export ("drawInRect:angle:")]
		void DrawInRect (CGRect rect, nfloat angle);

		[Export ("drawInBezierPath:angle:")]
		void DrawInBezierPath (NSBezierPath path, nfloat angle);

		[Export ("drawFromCenter:radius:toCenter:radius:options:")]
		void DrawFromCenterRadius (CGPoint startCenter, nfloat startRadius, CGPoint endCenter, nfloat endRadius, NSGradientDrawingOptions options);

		[Export ("drawInRect:relativeCenterPosition:")]
		void DrawInRect (CGRect rect, CGPoint relativeCenterPosition);

		[Export ("drawInBezierPath:relativeCenterPosition:")]
		void DrawInBezierPath (NSBezierPath path, CGPoint relativeCenterPosition);

		[Export ("colorSpace")]
		NSColorSpace ColorSpace { get; }

		[Export ("numberOfColorStops")]
		nint ColorStopsCount { get; }

		[Export ("getColor:location:atIndex:")]
		void GetColor (out NSColor color, out nfloat location, nint index);

		[Export ("interpolatedColorAtLocation:")]
		NSColor GetInterpolatedColor(nfloat location);
	}

	[ThreadSafe] // CurrentContext returns a context that can be used from the current thread
	[BaseType (typeof (NSObject))]
	public partial interface NSGraphicsContext {
		[Static, Export ("graphicsContextWithAttributes:")]
		NSGraphicsContext FromAttributes (NSDictionary attributes);
	
		[Static, Export ("graphicsContextWithWindow:")]
		NSGraphicsContext FromWindow (NSWindow window);
	
		[Static, Export ("graphicsContextWithBitmapImageRep:")]
		NSGraphicsContext FromBitmap (NSBitmapImageRep bitmapRep);
	
		[Static, Export ("graphicsContextWithGraphicsPort:flipped:")]
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'FromCGContext' instead.")]
		NSGraphicsContext FromGraphicsPort (IntPtr graphicsPort, bool initialFlippedState);
	
		[Static, Export ("currentContext")]
		NSGraphicsContext CurrentContext { get; set; }
	
		[Static, Export ("currentContextDrawingToScreen")]
		bool IsCurrentContextDrawingToScreen { get; }
	
		[Static, Export ("saveGraphicsState")]
		void GlobalSaveGraphicsState ();
	
		[Static, Export ("restoreGraphicsState")]
		void GlobalRestoreGraphicsState ();
	
		[Static, Export ("setGraphicsState:")]
		void SetGraphicsState (nint gState);
	
		[Export ("attributes")]
		NSDictionary Attributes { get; } 
	
		[Export ("isDrawingToScreen")]
		bool IsDrawingToScreen { get; }
	
		[Export ("saveGraphicsState")]
		void SaveGraphicsState ();
	
		[Export ("restoreGraphicsState")]
		void RestoreGraphicsState ();
	
		[Export ("flushGraphics")]
		void FlushGraphics ();

		// keep signature in sync with 'graphicsContextWithGraphicsPort:flipped:'
		[Export ("graphicsPort")]
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'CGContext' instead.")]
		IntPtr GraphicsPortHandle {get; }
	
		[Export ("isFlipped")]
		bool IsFlipped { get; }
	
		[Export ("shouldAntialias")]
		bool ShouldAntialias { get; set; }
	
		[Export ("imageInterpolation")]
		NSImageInterpolation ImageInterpolation { get; set; }
	
		[Export ("patternPhase")]
		CGPoint PatternPhase { get; set; }
	
		[Export ("compositingOperation")]
		NSComposite CompositingOperation { get; set; }
	
		[Export ("colorRenderingIntent")]
		NSColorRenderingIntent ColorRenderingIntent { get; set; }

		[Export ("CIContext")]
		MonoMac.CoreImage.CIContext CIContext { get; } 
		
		[Mac (10,10)]
		[Export ("CGContext")]
		CGContext CGContext { get; }

		[Mac (10,10)]
		[Static, Export ("graphicsContextWithCGContext:flipped:")]
		NSGraphicsContext FromCGContext (CGContext graphicsPort, bool initialFlippedState);
	}

	[BaseType (typeof (NSGraphicsContext))]
	[DisableDefaultCtor]
	public interface NSPrintPreviewGraphicsContext {
	}

	[BaseType (typeof (NSImageRep))]
	[DisableDefaultCtor] // An uncaught exception was raised: -[NSEPSImageRep init]: unrecognized selector sent to instance 0x1db2d90
	public interface NSEPSImageRep {
		[Static]
		[Export ("imageRepWithData:")]
		NSObject FromData (NSData epsData);

		[Export ("initWithData:")]
		IntPtr Constructor (NSData epsData);

		[Export ("prepareGState")]
		void PrepareGState ();

		[Export ("EPSRepresentation")]
		NSData EPSRepresentation { get; }

		[Export ("boundingBox")]
		CGRect BoundingBox { get; }
	}

	public delegate void GlobalEventHandler (NSEvent theEvent);
	public delegate NSEvent LocalEventHandler (NSEvent theEvent);
	public delegate void NSEventTrackHandler (nfloat gestureAmount, NSEventPhase eventPhase, bool isComplete, ref bool stop);

	[BaseType (typeof (NSObject))]
	public interface NSEvent {
		[Export ("type")]
		NSEventType Type { get; }

		[Export ("modifierFlags")]
		NSEventModifierMask ModifierFlags { get; }

		[Export ("timestamp")]
		double Timestamp { get; }

		[Export ("window")]
		NSWindow Window { get; }

		[Export ("windowNumber")]
		nint WindowNumber { get; }

		[Export ("context")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		NSGraphicsContext Context { get; }

		[Export ("clickCount")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nint ClickCount { get; }

		[Export ("buttonNumber")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nint ButtonNumber { get; }

		[Export ("eventNumber")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nint EventNumber { get; }

		[Export ("pressure")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nfloat Pressure { get; }

		[Export ("locationInWindow")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		CGPoint LocationInWindow { get; }

		[Export ("deltaX")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nfloat DeltaX { get; }

		[Export ("deltaY")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nfloat DeltaY { get; }

		[Export ("deltaZ")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nfloat DeltaZ { get; }

		[Export ("characters")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		string Characters { get; }

		[Export ("charactersIgnoringModifiers")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		string CharactersIgnoringModifiers { get; }

		[Export ("isARepeat")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		bool IsARepeat { get; }

		[Export ("keyCode")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		ushort KeyCode { get; }

		[Export ("trackingNumber")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nint TrackingNumber { get; }

		[Export ("userData")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		IntPtr UserData { get; }

		[Export ("trackingArea")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		NSTrackingArea TrackingArea { get; }

		[Export ("subtype")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		short Subtype { get; }

		[Export ("data1")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nint Data1 { get; }

		[Export ("data2")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nint Data2 { get; }

		//[Export ("eventRef")]
		//const void * EventRef ();

		[Static]
		[Export ("eventWithEventRef:")]
		NSEvent EventWithEventRef (IntPtr cgEventRef);

		[Export ("CGEvent")]
		IntPtr CGEvent { get; }

		[Static]
		[Export ("eventWithCGEvent:")]
		NSEvent EventWithCGEvent (IntPtr cgEventPtr);

		[Export ("magnification")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nfloat Magnification { get; }

		[Export ("deviceID")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nuint DeviceID { get; }

		[Export ("rotation")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nfloat Rotation { get; }

		[Export ("absoluteX")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nint AbsoluteX { get; }

		[Export ("absoluteY")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nint AbsoluteY { get; }

		[Export ("absoluteZ")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nint AbsoluteZ { get; }

		// TODO: What is the type?
		[Export ("buttonMask")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nuint ButtonMask { get; }

		[Export ("tilt")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		CGPoint Tilt { get; }

		[Export ("tangentialPressure")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nfloat TangentialPressure { get; }

		[Export ("vendorDefined")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		NSObject VendorDefined { get; }

		[Export ("vendorID")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nuint VendorID { get; }

		[Export ("tabletID")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nuint TabletID { get; }

		[Export ("pointingDeviceID")]
		nuint PointingDeviceID ();

		[Export ("systemTabletID")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nuint SystemTabletID { get; }

		[Export ("vendorPointingDeviceType")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nuint VendorPointingDeviceType { get; }

		[Export ("pointingDeviceSerialNumber")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nuint PointingDeviceSerialNumber { get; }

		[Export ("uniqueID")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		long UniqueID { get; }

		[Export ("capabilityMask")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nuint CapabilityMask { get; }

		[Export ("pointingDeviceType")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		NSPointingDeviceType PointingDeviceType { get; }

		[Export ("isEnteringProximity")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		bool IsEnteringProximity { get; }

		[Export ("touchesMatchingPhase:inView:")]
		NSSet TouchesMatchingPhase (NSTouchPhase phase, NSView view);

		[Static]
		[Export ("startPeriodicEventsAfterDelay:withPeriod:")]
		void StartPeriodicEventsAfterDelay (double delay, double period);

		[Static]
		[Export ("stopPeriodicEvents")]
		void StopPeriodicEvents ();

		[Static]
		[Export ("mouseEventWithType:location:modifierFlags:timestamp:windowNumber:context:eventNumber:clickCount:pressure:")]
		NSEvent MouseEvent (NSEventType type, CGPoint location, NSEventModifierMask flags, double time, nint wNum, [NullAllowed] NSGraphicsContext context, nint eNum, nint cNum, nfloat pressure);

		[Static]
		[Export ("keyEventWithType:location:modifierFlags:timestamp:windowNumber:context:characters:charactersIgnoringModifiers:isARepeat:keyCode:")]
		NSEvent KeyEvent (NSEventType type, CGPoint location, NSEventModifierMask flags, double time, nint wNum, [NullAllowed] NSGraphicsContext context, string keys, string ukeys, bool isARepeat, ushort code);

		[Static]
		[Export ("enterExitEventWithType:location:modifierFlags:timestamp:windowNumber:context:eventNumber:trackingNumber:userData:")]
		NSEvent EnterExitEvent (NSEventType type, CGPoint location, NSEventModifierMask flags, double time, nint wNum, [NullAllowed] NSGraphicsContext context, nint eNum, nint tNum, IntPtr data);

		[Static]
		[Export ("otherEventWithType:location:modifierFlags:timestamp:windowNumber:context:subtype:data1:data2:")]
		NSEvent OtherEvent (NSEventType type, CGPoint location, NSEventModifierMask flags, double time, nint wNum, [NullAllowed] NSGraphicsContext context, short subtype, nint d1, nint d2);

		[Static]
		[Export ("mouseLocation")]
		CGPoint CurrentMouseLocation { get; }

		[Static]
		[Export ("modifierFlags")]
		NSEventModifierMask CurrentModifierFlags { get; }

		[Static]
		[Export ("pressedMouseButtons")]
		nuint CurrentPressedMouseButtons { get; }

		[Static]
		[Export ("doubleClickInterval")]
		double DoubleClickInterval { get; }

		[Static]
		[Export ("keyRepeatDelay")]
		double KeyRepeatDelay { get; }

		[Static]
		[Export ("keyRepeatInterval")]
		double KeyRepeatInterval { get; }

		[Static]
		[Export ("addGlobalMonitorForEventsMatchingMask:handler:")]
		NSObject AddGlobalMonitorForEventsMatchingMask (NSEventMask mask, GlobalEventHandler handler);
		
		[Static]
		[Export ("addLocalMonitorForEventsMatchingMask:handler:")]
		NSObject AddLocalMonitorForEventsMatchingMask (NSEventMask mask, LocalEventHandler handler);
		
		[Static]
		[Export ("removeMonitor:")]
		void RemoveMonitor (NSObject eventMonitor);

		//Detected properties
		[Static]
		[Export ("mouseCoalescingEnabled")]
		bool MouseCoalescingEnabled { [Bind ("isMouseCoalescingEnabled")]get; set; }

		[Lion]
		[Export ("hasPreciseScrollingDeltas")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		bool HasPreciseScrollingDeltas { get; }

		[Lion]
		[Export ("scrollingDeltaX")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nfloat ScrollingDeltaX { get; }

		[Lion]
		[Export ("scrollingDeltaY")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		nfloat ScrollingDeltaY { get; }

		[Lion]
		[Export ("momentumPhase")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		NSEventPhase MomentumPhase { get; }

		[Lion]
		[Export ("isDirectionInvertedFromDevice")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		bool IsDirectionInvertedFromDevice { get; }

		[Lion]
		[Export ("phase")]
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		NSEventPhase Phase { get; }

		[Lion]
		[Static]
		[Export ("isSwipeTrackingFromScrollEventsEnabled")]
		bool IsSwipeTrackingFromScrollEventsEnabled { get; }

		[Lion]
		[Export ("trackSwipeEventWithOptions:dampenAmountThresholdMin:max:usingHandler:")]
		void TrackSwipeEvent (NSEventSwipeTrackingOptions options, nfloat minDampenThreshold, nfloat maxDampenThreshold, NSEventTrackHandler trackingHandler);
	}

	//[Mac (10,10)]
	[BaseType (typeof (NSObject), Delegates=new string [] {"WeakDelegate"}, Events=new Type[] {typeof (NSGestureRecognizerDelegate)})]
	public interface NSGestureRecognizer { //: NSCoding {
		[Export ("initWithTarget:action:")]
		IntPtr Constructor ([NullAllowed] NSObject target, [NullAllowed] Selector action);

		//[Export ("target", ArgumentSemantic.Weak), NullAllowed]
		[Export ("target"), NullAllowed]
		NSObject Target { get; set; }

		[Export ("action")]
		Selector Action { get; set; }

		//[Export ("state")]
		//NSGestureRecognizerState State { get; }

		[Export ("delegate")]//, ArgumentSemantic.Weak)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		//[Protocolize]
		NSGestureRecognizerDelegate Delegate { get; set; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		[Export ("view")]
		NSView View { get; }

		[Export ("delaysPrimaryMouseButtonEvents")]
		bool DelaysPrimaryMouseButtonEvents { get; set; }

		[Export ("delaysSecondaryMouseButtonEvents")]
		bool DelaysSecondaryMouseButtonEvents { get; set; }

		[Export ("delaysOtherMouseButtonEvents")]
		bool DelaysOtherMouseButtonEvents { get; set; }

		[Export ("delaysKeyEvents")]
		bool DelaysKeyEvents { get; set; }

		[Export ("delaysMagnificationEvents")]
		bool DelaysMagnificationEvents { get; set; }

		[Export ("delaysRotationEvents")]
		bool DelaysRotationEvents { get; set; }

		[Export ("locationInView:")]
		CGPoint LocationInView (NSView view);

		[Export ("reset")]
		void Reset ();

		[Export ("canPreventGestureRecognizer:")]
		bool CanPrevent (NSGestureRecognizer preventedGestureRecognizer);

		[Export ("canBePreventedByGestureRecognizer:")]
		bool CanBePrevented (NSGestureRecognizer preventingGestureRecognizer);

		[Export ("shouldRequireFailureOfGestureRecognizer:")]
		bool ShouldRequireFailureOfGestureRecognizer (NSGestureRecognizer otherGestureRecognizer);

		[Export ("shouldBeRequiredToFailByGestureRecognizer:")]
		bool ShouldBeRequiredToFailByGestureRecognizer (NSGestureRecognizer otherGestureRecognizer);

		[Export ("mouseDown:")]
		void MouseDown (NSEvent mouseEvent);

		[Export ("rightMouseDown:")]
		void RightMouseDown (NSEvent mouseEvent);

		[Export ("otherMouseDown:")]
		void OtherMouseDown (NSEvent mouseEvent);

		[Export ("mouseUp:")]
		void MouseUp (NSEvent mouseEvent);

		[Export ("rightMouseUp:")]
		void RightMouseUp (NSEvent mouseEvent);

		[Export ("otherMouseUp:")]
		void OtherMouseUp (NSEvent mouseEvent);

		[Export ("mouseDragged:")]
		void MouseDragged (NSEvent mouseEvent);

		[Export ("rightMouseDragged:")]
		void RightMouseDragged (NSEvent mouseEvent);

		[Export ("otherMouseDragged:")]
		void OtherMouseDragged (NSEvent mouseEvent);

		[Export ("keyDown:")]
		void KeyDown (NSEvent keyEvent);

		[Export ("keyUp:")]
		void KeyUp (NSEvent keyEvent);

		[Export ("flagsChanged:")]
		void FlagsChanged (NSEvent flagEvent);

		[Export ("tabletPoint:")]
		void TabletPoint (NSEvent tabletEvent);

		[Export ("magnifyWithEvent:")]
		void Magnify (NSEvent magnifyEvent);

		[Export ("rotateWithEvent:")]
		void Rotate (NSEvent rotateEvent);
/* 
#if XAMCORE_2_0
		[Mac (10,10,3, onlyOn64 : true)]
		[Export ("pressureChangeWithEvent:")]
		void PressureChange (NSEvent pressureChangeEvent);
#endif

		[Mac (10,11)]
		[Export ("pressureConfiguration", ArgumentSemantic.Strong)]
		NSPressureConfiguration PressureConfiguration { get; set; }

		[Mac (10,12,2)]
		[Export ("touchesBeganWithEvent:")]
		void TouchesBegan (NSEvent touchEvent);

		[Mac (10,12,2)]
		[Export ("touchesMovedWithEvent:")]
		void TouchesMoved (NSEvent touchEvent);

		[Mac (10,12,2)]
		[Export ("touchesEndedWithEvent:")]
		void TouchesEnded (NSEvent touchEvent);

		[Mac (10,12,2)]
		[Export ("touchesCancelledWithEvent:")]
		void TouchesCancelled (NSEvent touchEvent);
		*/
	}

	//[Mac (10,10)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	public interface NSGestureRecognizerDelegate {
		[Export ("gestureRecognizerShouldBegin:"), DelegateName ("NSGestureProbe"), DefaultValue (true)]
		bool ShouldBegin (NSGestureRecognizer gestureRecognizer);

		[Export ("gestureRecognizer:shouldRecognizeSimultaneouslyWithGestureRecognizer:"), DelegateName ("NSGesturesProbe"), DefaultValue (false)]
		bool ShouldRecognizeSimultaneously (NSGestureRecognizer gestureRecognizer, NSGestureRecognizer otherGestureRecognizer);

		[Export ("gestureRecognizer:shouldRequireFailureOfGestureRecognizer:"), DelegateName ("NSGesturesProbe"), DefaultValue (false)]
		bool ShouldRequireFailure (NSGestureRecognizer gestureRecognizer, NSGestureRecognizer otherGestureRecognizer);

		[Export ("gestureRecognizer:shouldBeRequiredToFailByGestureRecognizer:"), DelegateName ("NSGesturesProbe"), DefaultValue (false)]
		bool ShouldBeRequiredToFail (NSGestureRecognizer gestureRecognizer, NSGestureRecognizer otherGestureRecognizer);
/*
#if !XAMCORE_4_0
		[Export ("xamarinselector:removed:"), DelegateName ("NSGestureEvent"), DefaultValue (true)]
		[Obsolete ("It will never be called.")]
		bool ShouldReceiveEvent (NSGestureRecognizer gestureRecognizer, NSEvent gestureEvent);
#endif

		[Mac (10,11)]
		[Export ("gestureRecognizer:shouldAttemptToRecognizeWithEvent:"), DelegateName ("NSGestureEvent"), DefaultValue (true)]
		bool ShouldAttemptToRecognize (NSGestureRecognizer gestureRecognizer, NSEvent theEvent);

		[Mac (10,12,2)]
		[Export ("gestureRecognizer:shouldReceiveTouch:"), DelegateName ("NSTouchEvent"), DefaultValue (true)]
		bool ShouldReceiveTouch (NSGestureRecognizer gestureRecognizer, NSTouch touch);
		*/
	}


	[BaseType (typeof (NSObject))]
	[Dispose ("__mt_items_var = null;")]
	public partial interface NSMenu : NSAppearanceCustomization {
		[Export ("initWithTitle:")]
		IntPtr Constructor (string aTitle);

		[Static]
		[Export ("popUpContextMenu:withEvent:forView:")]
		void PopUpContextMenu (NSMenu menu, NSEvent theEvent, NSView view);

		[Static]
		[Export ("popUpContextMenu:withEvent:forView:withFont:")]
		void PopUpContextMenu (NSMenu menu, NSEvent theEvent, NSView view, [NullAllowed] NSFont font);

		[Export ("popUpMenuPositioningItem:atLocation:inView:")]
		bool PopUpMenu ([NullAllowed] NSMenuItem item, CGPoint location, [NullAllowed] NSView view);

		[Export ("insertItem:atIndex:")]
		[PostSnippet ("__mt_items_var = ItemArray();")]
		void InsertItem (NSMenuItem newItem, nint index);

		[Export ("addItem:")]
		[PostSnippet ("__mt_items_var = ItemArray();")]
		void AddItem (NSMenuItem newItem);

		[Export ("insertItemWithTitle:action:keyEquivalent:atIndex:")]
		[PostSnippet ("__mt_items_var = ItemArray();")]
		NSMenuItem InsertItem (string title, [NullAllowed] Selector action, string charCode, nint index);

		[Export ("addItemWithTitle:action:keyEquivalent:")]
		[PostSnippet ("__mt_items_var = ItemArray();")]
		NSMenuItem AddItem (string title, [NullAllowed] Selector action, string charCode);

		[Export ("removeItemAtIndex:")]
		[PostSnippet ("__mt_items_var = ItemArray();")]
		void RemoveItemAt (nint index);

		[Export ("removeItem:")]
		[PostSnippet ("__mt_items_var = ItemArray();")]
		void RemoveItem (NSMenuItem item);

		[Export ("setSubmenu:forItem:")]
		void SetSubmenu (NSMenu aMenu, NSMenuItem anItem);

		[Export ("removeAllItems")]
		[PostSnippet ("__mt_items_var = ItemArray();")]
		void RemoveAllItems ();

		[Export ("itemArray")]
		NSMenuItem [] ItemArray ();

		[Export ("numberOfItems")]
		nint Count { get; }

		[Export ("itemAtIndex:")]
		NSMenuItem ItemAt (nint index);

		[Export ("indexOfItem:")]
		nint IndexOf (NSMenuItem index);

		[Export ("indexOfItemWithTitle:")]
		nint IndexOf (string aTitle);

		[Export ("indexOfItemWithTag:")]
		nint IndexOf (nint itemTag);

		[Export ("indexOfItemWithRepresentedObject:")]
		nint IndexOfItem (NSObject obj);

		[Export ("indexOfItemWithSubmenu:")]
		nint IndexOfItem (NSMenu submenu);

		[Export ("indexOfItemWithTarget:andAction:")]
		nint IndexOfItem (NSObject target, Selector actionSelector);

		[Export ("itemWithTitle:")]
		NSMenuItem ItemWithTitle (string title);

		[Export ("itemWithTag:")]
		NSMenuItem ItemWithTag (nint tag);

		[Export ("update")]
		void Update ();

		[Export ("performKeyEquivalent:")]
		bool PerformKeyEquivalent (NSEvent theEvent);

		[Export ("itemChanged:")]
		void ItemChanged (NSMenuItem item);

		[Export ("performActionForItemAtIndex:")]
		void PerformActionForItem (nint index);

		[Export ("menuBarHeight")]
		nfloat MenuBarHeight { get; }

		[Export ("cancelTracking")]
		void CancelTracking ();

		[Export ("cancelTrackingWithoutAnimation")]
		void CancelTrackingWithoutAnimation ();

		[Export ("highlightedItem")]
		NSMenuItem HighlightedItem { get; }

		[Export ("size")]
		CGSize Size { get; }

		// TODO: Bind NSZone
		//[Static]
		//[Export ("menuZone")]
		//NSZone MenuZone { get; }

		[Export ("helpRequested:")]
		void HelpRequested (NSEvent eventPtr);

		[Export ("isTornOff")]
		bool IsTornOff { get; }

		//Detected properties
		[Export ("title")]
		string Title { get; set; }

		[Static]
		[Export ("menuBarVisible")]
		bool MenuBarVisible { get; set; }

		[Export ("supermenu")]
		NSMenu Supermenu { get; set; }

		[Export ("autoenablesItems")]
		bool AutoEnablesItems { get; set; }

		[Export ("delegate")]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSMenuDelegate Delegate { get; set; }
		
		[Export ("minimumWidth")]
		nfloat MinimumWidth { get; set; }

		[Export ("font")]
		NSFont Font { get; set; }

		[Export ("allowsContextMenuPlugIns")]
		bool AllowsContextMenuPlugIns { get; set; }

		[Export ("showsStateColumn")]
		bool ShowsStateColumn { get; set; }

		[Export ("menuChangedMessagesEnabled")]
		bool MenuChangedMessagesEnabled { get; set; }

		[Export ("propertiesToUpdate")]
		NSMenuProperty PropertiesToUpdate ();
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSMenuDelegate {
		[Export ("menuNeedsUpdate:")]
		void NeedsUpdate (NSMenu menu);

		[Export ("numberOfItemsInMenu:")]
		nint MenuItemCount (NSMenu menu);

		[Export ("menu:updateItem:atIndex:shouldCancel:")]
		bool UpdateItem (NSMenu menu, NSMenuItem item, nint atIndex, bool shouldCancel);

		[Export ("menuHasKeyEquivalent:forEvent:target:action:")]
		bool HasKeyEquivalentForEvent (NSMenu menu, NSEvent theEvent, NSObject target, Selector action);

		[Export ("menuWillOpen:")]
		void MenuWillOpen (NSMenu menu);

		[Export ("menuDidClose:")]
		void MenuDidClose (NSMenu menu);

		[Abstract]
		[Export ("menu:willHighlightItem:")]
		void MenuWillHighlightItem (NSMenu menu, NSMenuItem item);

		[Export ("confinementRectForMenu:onScreen:")]
		CGRect ConfinementRectForMenu (NSMenu menu, NSScreen screen);
	}

	[BaseType (typeof (NSObject))]
	public interface NSMenuItem {
		[Static]
		[Export ("separatorItem")]
		NSMenuItem SeparatorItem { get; }

		[Export ("initWithTitle:action:keyEquivalent:")]
		IntPtr Constructor (string title, [NullAllowed] Selector selectorAction, string charCode);

		[Export ("hasSubmenu")]
		bool HasSubmenu { get; }

		[Export ("parentItem")]
		NSMenuItem ParentItem { get; }

		[Export ("isSeparatorItem")]
		bool IsSeparatorItem { get; }

		[Export ("userKeyEquivalent")]
		string UserKeyEquivalent { get; }

		[Export ("setTitleWithMnemonic:")]
		void SetTitleWithMnemonic (string stringWithAmpersand);

		[Export ("isHighlighted")]
		bool Highlighted { get; }

		[Export ("isHiddenOrHasHiddenAncestor")]
		bool IsHiddenOrHasHiddenAncestor { get; }

		//Detected properties
		[Static]
		[Export ("usesUserKeyEquivalents")]
		bool UsesUserKeyEquivalents { get; set; }

		[Export ("menu")]
		NSMenu Menu { get; set; }

		[Export ("submenu"), NullAllowed]
		NSMenu Submenu { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("attributedTitle")]
		NSAttributedString AttributedTitle { get; set; }

		[Export ("keyEquivalent")]
		string KeyEquivalent { get; set; }

		[Export ("keyEquivalentModifierMask")]
		NSEventModifierMask KeyEquivalentModifierMask { get; set; }

		[Export ("image"), NullAllowed]
		NSImage Image { get; set; }

		[Export ("state")]
		NSCellStateValue State { get; set; }

		[Export ("onStateImage")]
		NSImage OnStateImage { get; set; }

		[Export ("offStateImage")]
		NSImage OffStateImage { get; set; }

		[Export ("mixedStateImage")]
		NSImage MixedStateImage { get; set; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")]get; set; }

		[Export ("alternate")]
		bool Alternate { [Bind ("isAlternate")]get; set; }

		[Export ("indentationLevel")]
		nint IndentationLevel { get; set; }

		[Export ("target"), NullAllowed]
		NSObject Target { get; set; }

		[Export ("action"), NullAllowed]
		Selector Action { get; set; }

		[Export ("tag")]
		nint Tag { get; set; }

		[Export ("representedObject")]
		NSObject RepresentedObject { get; set; }

		[Export ("view"), NullAllowed]
		NSView View { get; set; }

		[Export ("hidden")]
		bool Hidden { [Bind ("isHidden")]get; set; }

		[Export ("toolTip")]
		string ToolTip { get; set; }
	}

	[BaseType (typeof (NSButtonCell))]
	public interface NSMenuItemCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("calcSize")]
		void CalcSize ();

		[Export ("stateImageWidth")]
		nfloat StateImageWidth ();

		[Export ("imageWidth")]
		nfloat ImageWidth { get; }

		[Export ("titleWidth")]
		nfloat TitleWidth { get; }

		[Export ("keyEquivalentWidth")]
		nfloat KeyEquivalentWidth { get; }

		[Export ("stateImageRectForBounds:")]
		CGRect StateImageRectForBounds (CGRect cellFrame);

		[Export ("titleRectForBounds:")]
		CGRect TitleRectForBounds (CGRect cellFrame);

		[Export ("keyEquivalentRectForBounds:")]
		CGRect KeyEquivalentRectForBounds (CGRect cellFrame);

		[Export ("drawSeparatorItemWithFrame:inView:")]
		void DrawSeparatorItem (CGRect cellFrame, NSView controlView);

		[Export ("drawStateImageWithFrame:inView:")]
		void DrawStateImage (CGRect cellFrame, NSView controlView);

		[Export ("drawImageWithFrame:inView:")]
		void DrawImage (CGRect cellFrame, NSView controlView);

		[Export ("drawTitleWithFrame:inView:")]
		void DrawTitle (CGRect cellFrame, NSView controlView);

		[Export ("drawKeyEquivalentWithFrame:inView:")]
		void DrawKeyEquivalent (CGRect cellFrame, NSView controlView);

		[Export ("drawBorderAndBackgroundWithFrame:inView:")]
		void DrawBorderAndBackground (CGRect cellFrame, NSView controlView);

		[Export ("tag")]
		nint Tag { get; }

		//Detected properties
		[Export ("menuItem")]
		NSMenuItem MenuItem { get; set; }

		[Export ("menuView")]
		NSMenuView MenuView { get; set; }

		[Export ("needsSizing")]
		bool NeedsSizing { get; set; }

		[Export ("needsDisplay")]
		bool NeedsDisplay { get; set; }

	}

	[BaseType (typeof (NSView))]
	public interface NSMenuView {
		[Static]
		[Export ("menuBarHeight")]
		nfloat MenuBarHeight { get; }

		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frame);

		// <quote>Deprecated. Tear-off menus are not supported in OS X.</quote>
		//[Export ("initAsTearOff")]
		//IntPtr Constructor (int tokenInitAsTearOff);

		[Export ("itemChanged:")]
		void ItemChanged (NSNotification notification);

		[Export ("itemAdded:")]
		void ItemAdded (NSNotification notification);

		[Export ("itemRemoved:")]
		void ItemRemoved (NSNotification notification);

		[Export ("update")]
		void Update ();

		[Export ("innerRect")]
		CGRect InnerRect { get; }

		[Export ("rectOfItemAtIndex:")]
		CGRect RectOfItemAtIndex (nint index);

		[Export ("indexOfItemAtPoint:")]
		nint IndexOfItemAtPoint (CGPoint point);

		[Export ("setNeedsDisplayForItemAtIndex:")]
		void SetNeedsDisplay (nint itemAtIndex);

		[Export ("stateImageOffset")]
		nfloat StateImageOffset { get; }

		[Export ("stateImageWidth")]
		nfloat StateImageWidth { get; }

		[Export ("imageAndTitleOffset")]
		nfloat ImageAndTitleOffset { get; }

		[Export ("imageAndTitleWidth")]
		nfloat ImageAndTitleWidth { get; }

		[Export ("keyEquivalentOffset")]
		nfloat KeyEquivalentOffset { get; }

		[Export ("keyEquivalentWidth")]
		nfloat KeyEquivalentWidth { get; }

		[Export ("setMenuItemCell:forItemAtIndex:")]
		void SetMenuItemCell (NSMenuItemCell cell, nint itemAtIndex);

		[Export ("menuItemCellForItemAtIndex:")]
		NSMenuItemCell GetMenuItemCell (nint itemAtIndex);

		[Export ("attachedMenuView")]
		NSMenuView AttachedMenuView { get; }

		[Export ("sizeToFit")]
		void SizeToFit ();

		[Export ("attachedMenu")]
		NSMenu AttachedMenu { get; }

		[Export ("isAttached")]
		bool IsAttached { get; }

		[Export ("isTornOff")]
		bool IsTornOff { get; }

		[Export ("locationForSubmenu:")]
		CGPoint LocationForSubmenu (NSMenu aSubmenu);

		[Export ("setWindowFrameForAttachingToRect:onScreen:preferredEdge:popUpSelectedItem:")]
		void SetWindowFrameForAttachingToRect (CGRect screenRect, NSScreen onScreen, NSRectEdge preferredEdge, nint popupSelectedItem);

		[Export ("detachSubmenu")]
		void DetachSubmenu ();

		[Export ("attachSubmenuForItemAtIndex:")]
		void AttachSubmenuForItemAtIndex (nint index);

		[Export ("performActionWithHighlightingForItemAtIndex:")]
		void PerformActionWithHighlighting (nint forItemAtIndex);

		[Export ("trackWithEvent:")]
		bool TrackWithEvent (NSEvent theEvent);

		//Detected properties
		[Export ("menu")]
		NSMenu Menu { get; set; }

		[Export ("horizontal")]
		bool Horizontal { [Bind ("isHorizontal")]get; set; }

		[Export ("font")]
		NSFont Font { get; set; }

		[Export ("highlightedItemIndex")]
		nint HighlightedItemIndex { get; set; }

		[Export ("needsSizing")]
		bool NeedsSizing { get; set; }

		[Export ("horizontalEdgePadding")]
		nfloat HorizontalEdgePadding { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface NSNib {
		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl nibFileUrl);

		[Export ("initWithNibNamed:bundle:")]
		IntPtr Constructor (string nibName, NSBundle bundle);

		[Export ("instantiateNibWithExternalNameTable:")]
		bool InstantiateNib (NSDictionary externalNameTable);

		// This requires an "out NSArray"
		//[Export ("instantiateNibWithOwner:topLevelObjects:")]
		//bool InstantiateNib (NSObject owner, NSArray topLevelObjects);
	}	

	[BaseType (typeof (NSController))]
	public interface NSObjectController {
		[Export ("initWithContent:")]
		IntPtr Constructor (NSObject content);

		[Export ("content")]
		NSObject Content { get; set; }

		[Export ("selection")]
		NSObjectController Selection { get; }

		[Export ("selectedObjects")]
		NSObject [] SelectedObjects { get; [NotImplemented] set; }

		[Export ("automaticallyPreparesContent")]
		bool AutomaticallyPreparesContent { get; set; }

		[Export ("prepareContent")]
		void PrepareContent ();

		[Export ("objectClass")]
		Class ObjectClass { get; set; }

		// TODO: Geoff, can you review if we need to make this a [Factory]?
		[Export ("newObject")]
		NSObjectController NewObject { get; }

		[Export ("addObject:")]
		void AddObject (NSObject object1);

		[Export ("removeObject:")]
		void RemoveObject (NSObject object1);

		[Export ("setEditable:")]
		void SetEditable (bool editable);

		[Export ("editable")]
		bool Editable { [Bind ("isEditable")] get; set; }

		[Export ("add:")]
		void Add (NSObject sender);

		[Export ("canAdd")]
		bool CanAdd { get; }

		[Export ("remove:")]
		void Remove (NSObject sender);

		[Export ("canRemove")]
		bool CanRemove { get; }

		[Export ("validateUserInterfaceItem:")]
		bool ValidateUserInterfaceItem (NSObject item);

		//[Export ("managedObjectContext")]
		//NSManagedObjectContext ManagedObjectContext { get; set; }

		[Export ("entityName")]
		string EntityName { get; set; }

		[Export ("fetchPredicate")]
		NSPredicate FetchPredicate { get; set; }

		//[Export ("fetchWithRequest:merge:error:")]
		//bool FetchWithRequestMerge (NSFetchRequest fetchRequest, bool merge, NSError error);

		[Export ("fetch:")]
		void Fetch (NSObject sender);

		[Export ("usesLazyFetching")]
		bool UsesLazyFetching { get; set; }

		//[Export ("defaultFetchRequest")]
		//NSFetchRequest DefaultFetchRequest { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSOpenGLPixelFormat {
		[Export ("initWithData:")]
		IntPtr Constructor (NSData attribs);

		// TODO: wrap the CLContext and take a CLContext here instead.
		//[Export ("initWithCGLPixelFormatObj:")]
		//IntPtr Constructor (IntPtr cglContextHandle);

		[Export ("getValues:forAttribute:forVirtualScreen:")]
		void GetValue (ref int vals, NSOpenGLPixelFormatAttribute attrib, int screen);

		[Export ("numberOfVirtualScreens")]
		int NumberOfVirtualScreens { get; }

		[Export ("CGLPixelFormatObj")]
		CGLPixelFormat CGLPixelFormat { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSOpenGLPixelBuffer {
		[Export ("initWithTextureTarget:textureInternalFormat:textureMaxMipMapLevel:pixelsWide:pixelsHigh:")]
		IntPtr Constructor (NSGLTextureTarget targetGlEnum, NSGLFormat format, int maxLevel, int pixelsWide, int pixelsHigh);

		// FIXME: This conflicts with our internal ctor
		// [Export ("initWithCGLPBufferObj:")]
		// IntPtr Constructor (IntPtr pbuffer);

		[Export ("CGLPBufferObj")]
		IntPtr CGLPBuffer { get; }

		[Export ("pixelsWide")]
		int PixelsWide { get; }

		[Export ("pixelsHigh")]
		int PixelsHigh { get; }

		[Export ("textureTarget")]
		NSGLTextureTarget TextureTarget { get; }

		[Export ("textureInternalFormat")]
		NSGLFormat TextureInternalFormat { get; }

		[Export ("textureMaxMipMapLevel")]
		int TextureMaxMipMapLevel { get; }
	}

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // warns with "invalid context" at runtime
	public interface NSOpenGLContext {
		[Export ("initWithFormat:shareContext:")]
		IntPtr Constructor (NSOpenGLPixelFormat format, [NullAllowed] NSOpenGLContext shareContext);

		// FIXME: This conflicts with our internal ctor
		// [Export ("initWithCGLContextObj:")]
		// IntPtr Constructor (IntPtr cglContext);

		[Export ("setFullScreen")]
		void SetFullScreen ();

		[Export ("setOffScreen:width:height:rowbytes:")]
		void SetOffScreen (IntPtr baseaddr, int width, int height, int rowbytes);

		[Export ("clearDrawable")]
		void ClearDrawable ();

		[Export ("update")]
		void Update ();

		[ThreadSafe]
		[Export ("flushBuffer")]
		void FlushBuffer ();

                [ThreadSafe]
		[Export ("makeCurrentContext")]
		void MakeCurrentContext ();

		[Static]
		[Export ("clearCurrentContext")]
		void ClearCurrentContext ();

		[Static]
		[Export ("currentContext")]
		NSOpenGLContext CurrentContext { get; }

		[Export ("copyAttributesFromContext:withMask:")]
		void CopyAttributes (NSOpenGLContext context, uint mask);

		[Export ("setValues:forParameter:")]
		void SetValues (IntPtr vals, NSOpenGLContextParameter param);

		[Export ("getValues:forParameter:")]
		void GetValues (IntPtr vals, NSOpenGLContextParameter param);

		[Export ("createTexture:fromView:internalFormat:")]
		void CreateTexture (int targetIdentifier, NSView view, int format);

		[ThreadSafe]
		[Export ("CGLContextObj")]
		CGLContext CGLContext { get; }

		[Export ("setPixelBuffer:cubeMapFace:mipMapLevel:currentVirtualScreen:")]
		void SetPixelBuffer (NSOpenGLPixelBuffer pixelBuffer, NSGLTextureCubeMap face, int level, int screen);

		[Export ("pixelBuffer")]
		NSOpenGLPixelBuffer PixelBuffer { get; }

		[Export ("pixelBufferCubeMapFace")]
		int PixelBufferCubeMapFace { get; }

		[Export ("pixelBufferMipMapLevel")]
		int PixelBufferMipMapLevel { get; }

		// TODO: fixme enumerations
		// GL_FRONT, GL_BACK, GL_AUX0
		[Export ("setTextureImageToPixelBuffer:colorBuffer:")]
		void SetTextureImage (NSOpenGLPixelBuffer pixelBuffer, NSGLColorBuffer source);

		//Detected properties
		[Export ("view")]
		NSView View { get; set; }

		[Export ("currentVirtualScreen")]
		int CurrentVirtualScreen { get; set; }
	}

	[BaseType (typeof (NSView))]
	public partial interface NSOpenGLView {
		[Static]
		[Export ("defaultPixelFormat")]
		NSOpenGLPixelFormat DefaultPixelFormat { get; }

		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("initWithFrame:pixelFormat:")]
		IntPtr Constructor (CGRect frameRect, NSOpenGLPixelFormat format);

		[Export ("clearGLContext")]
		void ClearGLContext ();

		[Export ("update")]
		void Update ();

		[Export ("reshape")]
		void Reshape ();

		[Export ("prepareOpenGL")]
		void PrepareOpenGL ();

		//Detected properties
		[Export ("openGLContext")]
		NSOpenGLContext OpenGLContext { get; set; }

		[Export ("pixelFormat")]
		NSOpenGLPixelFormat PixelFormat { get; set; }
	}

	[BaseType (typeof (NSSavePanel))]
	public interface NSOpenPanel {
		[Static]
		[Export ("openPanel")]
		NSOpenPanel OpenPanel { get; }

		[Export ("URLs")]
		NSUrl [] Urls { get; }

		//Detected properties
		[Export ("resolvesAliases")]
		bool ResolvesAliases { get; set; }

		[Export ("canChooseDirectories")]
		bool CanChooseDirectories { get; set; }

		[Export ("allowsMultipleSelection")]
		bool AllowsMultipleSelection { get; set; }

		[Export ("canChooseFiles")]
		bool CanChooseFiles { get; set; }

		// Deprecated methods, but needed to run on pre 10.6 systems
		[Deprecated (PlatformName.MacOSX, 10, 6, message: "use Uris")]
		[Export ("filenames")]
		string [] Filenames { get; }

		//runModalForWindows:Completeion
		[Deprecated (PlatformName.MacOSX, 10, 6, message: "use runModalForWindow:")]
		[Export ("beginSheetForDirectory:file:types:modalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		void BeginSheet ([NullAllowed] string directory, [NullAllowed] string fileName, [NullAllowed] string [] fileTypes, [NullAllowed] NSWindow modalForWindow, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);

		[Deprecated (PlatformName.MacOSX, 10, 6, message: "use runWithCompletionHandler:")]
		[Export ("beginForDirectory:file:types:modelessDelegate:didEndSelector:contextInfo:")]
		void Begin ([NullAllowed] string directory, [NullAllowed] string fileName, [NullAllowed] string [] fileTypes, [NullAllowed] NSObject modelessDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);
		
		[Deprecated (PlatformName.MacOSX, 10, 6, message: "use runModal:")]
		[Export ("runModalForDirectory:file:types:")]
		nint RunModal ([NullAllowed] string directory, [NullAllowed] string fileName, [NullAllowed] string [] types);

		[Deprecated (PlatformName.MacOSX, 10, 6, message: "use runModal:")]
		[Export ("runModalForTypes:")]
		nint RunModal (string [] types);
	}

	[BaseType (typeof (NSOpenPanel))]
	[DisableDefaultCtor] // should not be created by (only returned to) user code
	public interface NSRemoteOpenPanel {}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSOpenSavePanelDelegate {
		[Export ("panel:shouldEnableURL:"), DelegateName ("NSOpenSavePanelUrl"), DefaultValue (true)]
		bool ShouldEnableUrl (NSSavePanel panel, NSUrl url);

		[Export ("panel:validateURL:error:"), DelegateName ("NSOpenSavePanelValidate"), DefaultValue (true)]
		bool ValidateUrl (NSSavePanel panel, NSUrl url, out NSError outError);

		[Export ("panel:didChangeToDirectoryURL:"), EventArgs ("NSOpenSavePanelUrl")]
		void DidChangeToDirectory (NSSavePanel panel, NSUrl newDirectoryUrl);

		[Export ("panel:userEnteredFilename:confirmed:"), DelegateName ("NSOpenSaveFilenameConfirmation"), DefaultValueFromArgument ("filename")]
		string UserEnteredFilename (NSSavePanel panel, string filename, bool confirmed);

		[Export ("panel:willExpand:"), EventArgs ("NSOpenSaveExpanding")]
		void WillExpand (NSSavePanel panel, bool expanding);

		[Export ("panelSelectionDidChange:"), EventArgs ("NSOpenSaveSelectionChanged")]
		void SelectionDidChange (NSSavePanel panel);

		[Deprecated (PlatformName.MacOSX, 10, 6, message: "use ValidateUrlError")]
		[Export ("panel:isValidFilename:"), DelegateName ("NSOpenSaveFilename"), DefaultValue (true)]
		bool IsValidFilename (NSSavePanel panel, string fileName);

		[Deprecated (PlatformName.MacOSX, 10, 6, message: "Use DidChangeToDirectoryUrl instead")]
		[Export ("panel:directoryDidChange:"), EventArgs ("NSOpenSaveFilename")]
		void DirectoryDidChange (NSSavePanel panel, string path);

		[Deprecated (PlatformName.MacOSX, 10, 6, message: "this method is obsolete and does not control sorting order")]
		[Export ("panel:compareFilename:with:caseSensitive"), DelegateName ("NSOpenSaveCompare"), DefaultValue (NSComparisonResult.Same)]
		NSComparisonResult CompareFilenames (NSSavePanel panel, string name1, string name2, bool caseSensitive);

		[Deprecated (PlatformName.MacOSX, 10, 6, message: "use ShouldEnableUrl")]
		[Export ("panel:shouldShowFilename:"), DelegateName ("NSOpenSaveFilename"), DefaultValue (true)]
		bool ShouldShowFilename (NSSavePanel panel, string filename);
	}

	
	[BaseType (typeof (NSTableView))]
	public partial interface NSOutlineView {
		[Export ("outlineTableColumn"), NullAllowed]
		NSTableColumn OutlineTableColumn { get; set; }

		[Export ("isExpandable:")]
		bool IsExpandable (NSObject item);

		[Export ("expandItem:expandChildren:")]
		void ExpandItem ([NullAllowed] NSObject item, bool expandChildren);

		[Export ("expandItem:")]
		void ExpandItem (NSObject item);

		[Export ("collapseItem:collapseChildren:")]
		void CollapseItem ([NullAllowed] NSObject item, bool collapseChildren);

		[Export ("collapseItem:")]
		void CollapseItem (NSObject item);

		[Export ("reloadItem:reloadChildren:")]
		void ReloadItem ([NullAllowed] NSObject item, bool reloadChildren);

		[Export ("reloadItem:")]
		void ReloadItem (NSObject item);

		[Export ("parentForItem:")]
		NSObject GetParent (NSObject item);

		[Export ("itemAtRow:")]
		NSObject ItemAtRow (nint row);

		[Export ("rowForItem:")]
		nint RowForItem (NSObject item);

		[Export ("levelForItem:")]
		nint LevelForItem ([NullAllowed] NSObject item);

		[Export ("levelForRow:")]
		nint LevelForRow (nint row);

		[Export ("isItemExpanded:")]
		bool IsItemExpanded (NSObject item);

		[Export ("indentationPerLevel")]
		nfloat IndentationPerLevel { get; set; }

		[Export ("indentationMarkerFollowsCell")]
		bool IndentationMarkerFollowsCell { get; set; }

		[Export ("autoresizesOutlineColumn")]
		bool AutoresizesOutlineColumn { get; set; }

		[Export ("frameOfOutlineCellAtRow:")]
		CGRect FrameOfOutlineCellAtRow (nint row);

		[Export ("setDropItem:dropChildIndex:")]
		void SetDropItem ([NullAllowed] NSObject item, nint index);

		[Export ("shouldCollapseAutoExpandedItemsForDeposited:")]
		bool ShouldCollapseAutoExpandedItems (bool forDeposited);

		[Export ("autosaveExpandedItems")]
		bool AutosaveExpandedItems { get; set; }

		[Export ("delegate")]
		NSObject WeakDelegate  { get; set; }

		[Wrap ("WeakDelegate")]
		NSOutlineViewDelegate Delegate  { get; set; }

		[Export ("dataSource")]
		NSObject WeakDataSource  { get; set; }

		[Wrap ("WeakDataSource")]
		NSOutlineViewDataSource DataSource  { get; set; }
		
		[Introduced (PlatformName.MacOSX, 10,7)]
		[Export ("userInterfaceLayoutDirection")]
		NSUserInterfaceLayoutDirection UserInterfaceLayoutDirection { get; set; }

		[Introduced (PlatformName.MacOSX, 10, 10)]
		[Export ("numberOfChildrenOfItem:")]
		nint NumberOfChildren ([NullAllowed] NSObject item);

		[Introduced (PlatformName.MacOSX, 10,10)]
		[Export ("child:ofItem:")]
		NSObject GetChild (nint index, [NullAllowed] NSObject parentItem);

		[Introduced (PlatformName.MacOSX, 10,11)]
		[Export ("childIndexForItem:")]
		nint GetChildIndex (NSObject item);

		[Introduced (PlatformName.MacOSX, 10, 12)]
		[Export ("stronglyReferencesItems")]
		bool StronglyReferencesItems { get; set; }		
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public partial interface NSOutlineViewDelegate {
		[Export ("outlineView:willDisplayCell:forTableColumn:item:")]
		void WillDisplayCell (NSOutlineView outlineView, NSObject cell, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:shouldEditTableColumn:item:")] [DefaultValue (false)]
		bool ShouldEditTableColumn (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item);
	
		[Export ("selectionShouldChangeInOutlineView:")] [DefaultValue (false)]
		bool SelectionShouldChange (NSOutlineView outlineView);
	
		[Export ("outlineView:shouldSelectItem:")] [DefaultValue (true)]
		bool ShouldSelectItem (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:selectionIndexesForProposedSelection:")]
		NSIndexSet GetSelectionIndexes (NSOutlineView outlineView, NSIndexSet proposedSelectionIndexes);
	
		[Export ("outlineView:shouldSelectTableColumn:")]
		bool ShouldSelectTableColumn (NSOutlineView outlineView, NSTableColumn tableColumn);
	
		[Export ("outlineView:mouseDownInHeaderOfTableColumn:")]
		void MouseDown (NSOutlineView outlineView, NSTableColumn tableColumn);
	
		[Export ("outlineView:didClickTableColumn:")]
		void DidClickTableColumn (NSOutlineView outlineView, NSTableColumn tableColumn);
	
		[Export ("outlineView:didDragTableColumn:")]
		void DidDragTableColumn (NSOutlineView outlineView, NSTableColumn tableColumn);
		
		[Export ("outlineView:toolTipForCell:rect:tableColumn:item:mouseLocation:")]
		string ToolTipForCell (NSOutlineView outlineView, NSCell cell, ref CGRect rect, NSTableColumn tableColumn, NSObject item, CGPoint mouseLocation);
	
		[Export ("outlineView:heightOfRowByItem:"), NoDefaultValue]
		nfloat GetRowHeight (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:typeSelectStringForTableColumn:item:")]
		string GetSelectString (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:nextTypeSelectMatchFromItem:toItem:forString:")]
		NSObject GetNextTypeSelectMatch (NSOutlineView outlineView, NSObject startItem, NSObject endItem, string searchString);
	
		[Export ("outlineView:shouldTypeSelectForEvent:withCurrentSearchString:")]
		bool ShouldTypeSelect (NSOutlineView outlineView, NSEvent theEvent, string searchString);
	
		[Export ("outlineView:shouldShowCellExpansionForTableColumn:item:")]
		bool ShouldShowCellExpansion (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:shouldTrackCell:forTableColumn:item:")]
		bool ShouldTrackCell (NSOutlineView outlineView, NSCell cell, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:dataCellForTableColumn:item:"), NoDefaultValue]
		NSCell GetCell (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item);

		[Export ("outlineView:viewForTableColumn:item:"), NoDefaultValue]
		NSView GetView (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:isGroupItem:")]
		bool IsGroupItem (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:shouldExpandItem:")]
		bool ShouldExpandItem (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:shouldCollapseItem:")]
		bool ShouldCollapseItem (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:willDisplayOutlineCell:forTableColumn:item:")]
		void WillDisplayOutlineCell (NSOutlineView outlineView, NSObject cell, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:sizeToFitWidthOfColumn:"), NoDefaultValue]
		nfloat GetSizeToFitColumnWidth (NSOutlineView outlineView, nint column);
	
		[Export ("outlineView:shouldReorderColumn:toColumn:")]
		bool ShouldReorder (NSOutlineView outlineView, nint columnIndex, nint newColumnIndex);
	
		[Export ("outlineView:shouldShowOutlineCellForItem:")]
		bool ShouldShowOutlineCell (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineViewColumnDidMove:")]
		void ColumnDidMove (NSNotification notification);
	
		[Export ("outlineViewColumnDidResize:")]
		void ColumnDidResize (NSNotification notification);
	
		[Export ("outlineViewSelectionIsChanging:")]
		void SelectionIsChanging (NSNotification notification);
	
		[Export ("outlineViewItemWillExpand:")]
		void ItemWillExpand (NSNotification notification);
	
		[Export ("outlineViewItemDidExpand:")]
		void ItemDidExpand (NSNotification notification);
	
		[Export ("outlineViewItemWillCollapse:")]
		void ItemWillCollapse (NSNotification notification);
	
		[Export ("outlineViewItemDidCollapse:")]
		void ItemDidCollapse (NSNotification notification);

		[Export ("outlineViewSelectionDidChange:")]
		void SelectionDidChange (NSNotification notification);
		
		[Lion]
		[Export ("outlineView:didAddRowView:forRow:"), EventArgs ("NSTableViewRow")]
		void DidAddRowView (NSOutlineView outlineView, NSTableRowView rowView, nint row);

		[Lion]
		[Export ("outlineView:didRemoveRowView:forRow:"), EventArgs ("NSTableViewRow")]
		void DidRemoveRowView (NSOutlineView outlineView, NSTableRowView rowView, nint row);

	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	public partial interface NSOutlineViewDataSource {
		[Export ("outlineView:child:ofItem:")]
		NSObject GetChild (NSOutlineView outlineView, nint childIndex, NSObject item);
	
		[Export ("outlineView:isItemExpandable:")]
		bool ItemExpandable (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:numberOfChildrenOfItem:")]
		nint GetChildrenCount (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:objectValueForTableColumn:byItem:")]
		NSObject GetObjectValue (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:setObjectValue:forTableColumn:byItem:")]
		void SetObjectValue (NSOutlineView outlineView, NSObject theObject, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:itemForPersistentObject:")]
		NSObject ItemForPersistentObject (NSOutlineView outlineView, NSObject theObject);
	
		[Export ("outlineView:persistentObjectForItem:")]
		NSObject PersistentObjectForItem (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:sortDescriptorsDidChange:")]
		void SortDescriptorsChanged (NSOutlineView outlineView, NSSortDescriptor [] oldDescriptors);
	
		[Export ("outlineView:writeItems:toPasteboard:")]
		bool OutlineViewwriteItemstoPasteboard (NSOutlineView outlineView, NSArray items, NSPasteboard pboard);
	
		[Export ("outlineView:validateDrop:proposedItem:proposedChildIndex:")]
		NSDragOperation ValidateDrop (NSOutlineView outlineView, NSDraggingInfo info, NSObject item, nint index);
	
		[Export ("outlineView:acceptDrop:item:childIndex:")]
		bool AcceptDrop (NSOutlineView outlineView, NSDraggingInfo info, NSObject item, nint index);
	
		[Export ("outlineView:namesOfPromisedFilesDroppedAtDestination:forDraggedItems:")]
		string [] FilesDropped (NSOutlineView outlineView, NSUrl dropDestination, NSArray items);
		
		// - (id <NSPasteboardWriting>)outlineView:(NSOutlineView *)outlineView pasteboardWriterForItem:(id)item NS_AVAILABLE_MAC(10_7);
		[Export ("outlineView:pasteboardWriterForItem:")]
		NSPasteboardWriting PasteboardWriterForItem (NSOutlineView outlineView, NSObject item);

		// - (void)outlineView:(NSOutlineView *)outlineView draggingSession:(NSDraggingSession *)session willBeginAtPoint:(NSPoint)screenPoint forItems:(NSArray *)draggedItems NS_AVAILABLE_MAC(10_7);
		[Export ("outlineView:draggingSession:willBeginAtPoint:forItems:")]
		void DraggingSessionWillBegin (NSOutlineView outlineView, NSDraggingSession session, CGPoint screenPoint, NSArray draggedItems);

		// - (void)outlineView:(NSOutlineView *)outlineView draggingSession:(NSDraggingSession *)session endedAtPoint:(NSPoint)screenPoint operation:(NSDragOperation)operation NS_AVAILABLE_MAC(10_7);
		[Export ("outlineView:draggingSession:endedAtPoint:operation:")]
		void DraggingSessionEnded (NSOutlineView outlineView, NSDraggingSession session, CGPoint screenPoint, NSDragOperation operation);

		// - (void)outlineView:(NSOutlineView *)outlineView updateDraggingItemsForDrag:(id <NSDraggingInfo>)draggingInfo NS_AVAILABLE_MAC(10_7);
		[Export ("outlineView:updateDraggingItemsForDrag:")]
		void UpdateDraggingItemsForDrag (NSOutlineView outlineView, NSDraggingInfo draggingInfo);
	}
	

	[BaseType (typeof (NSObject))]
	public partial interface NSHelpManager {
		[Static]
		[Export ("sharedHelpManager")]
		NSHelpManager SharedHelpManager ();

		[Export ("setContextHelp:forObject:")]
		void SetContext (NSAttributedString attrString, NSObject theObject);

		[Export ("removeContextHelpForObject:")]
		void RemoveContext (NSObject theObject);

		[Export ("contextHelpForObject:")]
		NSAttributedString Context (NSObject theObject);

		[Export ("showContextHelpForObject:locationHint:")]
		bool ShowContext (NSObject theObject, CGPoint pt);

		[Export ("openHelpAnchor:inBook:")]
		void OpenHelpAnchor (string anchor, string book);

		[Export ("findString:inBook:")]
		void FindString (string query, string book);

		[Export ("registerBooksInBundle:")]
		bool RegisterBooks (NSBundle bundle );

		//Detected properties
		[Static]
		[Export ("contextHelpModeActive")]
		bool ContextHelpModeActive { [Bind ("isContextHelpModeActive")]get; set; }
	}

	[BaseType (typeof (NSObject), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSImageDelegate)})]
	[Dispose ("__mt_reps_var = null;")]
	public partial interface NSImage {
		[Static]
		[Export ("imageNamed:")]
		NSImage ImageNamed (string name);

		[Export ("initWithSize:")]
		IntPtr Constructor (CGSize aSize);

		[Export ("initWithData:")]
		IntPtr Constructor (NSData data);

		[Export ("initWithContentsOfFile:")]
		IntPtr Constructor (string fileName);

		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl url);

		//[Export ("initByReferencingFile:")]
		//IntPtr Constructor (string fileName);
		//[Export ("initByReferencingURL:")]
		//IntPtr Constructor (NSUrl url);

		// FIXME: need IconRec
		//[Export ("initWithIconRef:")]
		//IntPtr Constructor (IconRef iconRef);

		[Export ("initWithPasteboard:")]
		IntPtr Constructor (NSPasteboard pasteboard);

		//[Export ("initWithDataIgnoringOrientation:")]
		//IntPtr Constructor (NSData data);

		[Export ("drawAtPoint:fromRect:operation:fraction:")]
		void Draw (CGPoint point, CGRect fromRect, NSCompositingOperation op, nfloat delta);

		[Export ("drawInRect:fromRect:operation:fraction:")]
		void Draw (CGRect rect, CGRect fromRect, NSCompositingOperation op, nfloat delta);

		[Export ("drawInRect:fromRect:operation:fraction:respectFlipped:hints:")]
		void Draw (CGRect dstSpacePortionRect, CGRect srcSpacePortionRect, NSCompositingOperation op, nfloat requestedAlpha, bool respectContextIsFlipped, [NullAllowed] NSDictionary hints);

		[Export ("drawRepresentation:inRect:")]
		bool Draw (NSImageRep imageRep, CGRect rect);

		[Export ("recache")]
		void Recache ();

		[Export ("TIFFRepresentation")]
		NSData AsTiff ();

		[Export ("TIFFRepresentationUsingCompression:factor:")]
		NSData AsTiff (NSTiffCompression comp, nfloat aFloat);

		[Export ("representations")]
		NSImageRep [] Representations ();

		[Export ("addRepresentations:")]
		[PostSnippet ("__mt_reps_var = Representations();")]
		void AddRepresentations (NSImageRep [] imageReps);

		[Export ("addRepresentation:")]
		[PostSnippet ("__mt_reps_var = Representations();")]
		void AddRepresentation (NSImageRep imageRep);

		[Export ("removeRepresentation:")]
		[PostSnippet ("__mt_reps_var = Representations();")]
		void RemoveRepresentation (NSImageRep imageRep);

		[Export ("isValid")]
		bool IsValid { get; }

		[Export ("lockFocus")]
		void LockFocus ();

		[Export ("lockFocusFlipped:")]
		void LockFocusFlipped (bool flipped);

		[Export ("unlockFocus")]
		void UnlockFocus ();

		[Deprecated (PlatformName.MacOSX, 10, 6, message: "Use BestRepresentation instead")]
		[Export ("bestRepresentationForDevice:")]
		NSImageRep BestRepresentationForDevice ([NullAllowed] NSDictionary deviceDescription);

		[Static]
		[Export ("imageUnfilteredFileTypes")]
		NSObject [] ImageUnfilteredFileTypes ();

		[Static]
		[Export ("imageUnfilteredPasteboardTypes")]
		string [] ImageUnfilteredPasteboardTypes ();

		[Static]
		[Export ("imageFileTypes")]
		string [] ImageFileTypes { get; }

		[Static]
		[Export ("imagePasteboardTypes")]
		string [] ImagePasteboardTypes { get; }
		
		[Static]
		[Export ("imageTypes")]
		string [] ImageTypes { get; }

		[Static]
		[Export ("imageUnfilteredTypes")]
		string [] ImageUnfilteredTypes { get; }
		
		[Static]
		[Export ("canInitWithPasteboard:")]
		bool CanInitWithPasteboard (NSPasteboard pasteboard);

		[Export ("cancelIncrementalLoad")]
		void CancelIncrementalLoad ();

		[Export ("accessibilityDescription")]
		string AccessibilityDescription	 { get; set; }

		[Export ("initWithCGImage:size:")]
		IntPtr Constructor (CGImage cgImage, CGSize size);

		[Export ("CGImageForProposedRect:context:hints:")]
		CGImage AsCGImage (ref CGRect proposedDestRect, [NullAllowed] NSGraphicsContext referenceContext, [NullAllowed] NSDictionary hints);

		[Export ("bestRepresentationForRect:context:hints:")]
		NSImageRep BestRepresentation (CGRect rect, [NullAllowed] NSGraphicsContext referenceContext, [NullAllowed] NSDictionary hints);

		[Export ("hitTestRect:withImageDestinationRect:context:hints:flipped:")]
		bool HitTestRect (CGRect testRectDestSpace, CGRect imageRectDestSpace, NSGraphicsContext context, NSDictionary hints, bool flipped);

		//Detected properties
		[Export ("size")]
		CGSize Size { get; set; }

		[Export ("name"), Internal]
		string GetName ();

		[Export ("setName:"), Internal]
		bool SetName (string aString);

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("usesEPSOnResolutionMismatch")]
		bool UsesEpsOnResolutionMismatch { get; set; }

		[Export ("prefersColorMatch")]
		bool PrefersColorMatch { get; set; }

		[Export ("matchesOnMultipleResolution")]
		bool MatchesOnMultipleResolution { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSImageDelegate Delegate { get; set; }

		[Export ("cacheMode")]
		NSImageCacheMode CacheMode { get; set; }

		[Export ("alignmentRect")]
		CGRect AlignmentRect { get; set; }

		[Export ("template")]
		bool Template { [Bind ("isTemplate")]get; set; }

		[Bind ("sizeWithAttributes:")]
		CGSize StringSize ([Target] string str, NSDictionary attributes);

		[Bind ("drawInRect:withAttributes:")]
		void DrawInRect ([Target] string str, CGRect rect, NSDictionary attributes);

		[Export ("drawInRect:fromRect:operation:fraction:")]
		void DrawInRect (CGRect dstRect, CGRect srcRect, NSCompositingOperation operation, nfloat delta);
		
		[Deprecated (PlatformName.MacOSX, 10, 6, message: "use DrawInRect with respectContextIsFlipped instead"), Export ("flipped")]
		bool Flipped { [Bind ("isFlipped")] get; set; }

		[Internal, Field ("NSImageNameQuickLookTemplate")]
		NSString NSImageNameQuickLookTemplate { get; }

		[Internal, Field ("NSImageNameBluetoothTemplate")]
		NSString NSImageNameBluetoothTemplate { get; }

		[Internal, Field ("NSImageNameIChatTheaterTemplate")]
		NSString NSImageNameIChatTheaterTemplate { get; }

		[Internal, Field ("NSImageNameSlideshowTemplate")]
		NSString NSImageNameSlideshowTemplate { get; }

		[Internal, Field ("NSImageNameActionTemplate")]
		NSString NSImageNameActionTemplate { get; }

		[Internal, Field ("NSImageNameSmartBadgeTemplate")]
		NSString NSImageNameSmartBadgeTemplate { get; }

		[Internal, Field ("NSImageNamePathTemplate")]
		NSString NSImageNamePathTemplate { get; }

		[Internal, Field ("NSImageNameInvalidDataFreestandingTemplate")]
		NSString NSImageNameInvalidDataFreestandingTemplate { get; }

		[Internal, Field ("NSImageNameLockLockedTemplate")]
		NSString NSImageNameLockLockedTemplate { get; }

		[Internal, Field ("NSImageNameLockUnlockedTemplate")]
		NSString NSImageNameLockUnlockedTemplate { get; }

		[Internal, Field ("NSImageNameGoRightTemplate")]
		NSString NSImageNameGoRightTemplate { get; }

		[Internal, Field ("NSImageNameGoLeftTemplate")]
		NSString NSImageNameGoLeftTemplate { get; }

		[Internal, Field ("NSImageNameRightFacingTriangleTemplate")]
		NSString NSImageNameRightFacingTriangleTemplate { get; }

		[Internal, Field ("NSImageNameLeftFacingTriangleTemplate")]
		NSString NSImageNameLeftFacingTriangleTemplate { get; }

		[Internal, Field ("NSImageNameAddTemplate")]
		NSString NSImageNameAddTemplate { get; }

		[Internal, Field ("NSImageNameRemoveTemplate")]
		NSString NSImageNameRemoveTemplate { get; }

		[Internal, Field ("NSImageNameRevealFreestandingTemplate")]
		NSString NSImageNameRevealFreestandingTemplate { get; }

		[Internal, Field ("NSImageNameFollowLinkFreestandingTemplate")]
		NSString NSImageNameFollowLinkFreestandingTemplate { get; }

		[Internal, Field ("NSImageNameEnterFullScreenTemplate")]
		NSString NSImageNameEnterFullScreenTemplate { get; }

		[Internal, Field ("NSImageNameExitFullScreenTemplate")]
		NSString NSImageNameExitFullScreenTemplate { get; }

		[Internal, Field ("NSImageNameStopProgressTemplate")]
		NSString NSImageNameStopProgressTemplate { get; }

		[Internal, Field ("NSImageNameStopProgressFreestandingTemplate")]
		NSString NSImageNameStopProgressFreestandingTemplate { get; }

		[Internal, Field ("NSImageNameRefreshTemplate")]
		NSString NSImageNameRefreshTemplate { get; }

		[Internal, Field ("NSImageNameRefreshFreestandingTemplate")]
		NSString NSImageNameRefreshFreestandingTemplate { get; }

		[Internal, Field ("NSImageNameFolder")]
		NSString NSImageNameFolder { get; }

		[Internal, Field ("NSImageNameTrashEmpty")]
		NSString NSImageNameTrashEmpty { get; }

		[Internal, Field ("NSImageNameTrashFull")]
		NSString NSImageNameTrashFull { get; }

		[Internal, Field ("NSImageNameHomeTemplate")]
		NSString NSImageNameHomeTemplate { get; }

		[Internal, Field ("NSImageNameBookmarksTemplate")]
		NSString NSImageNameBookmarksTemplate { get; }

		[Internal, Field ("NSImageNameCaution")]
		NSString NSImageNameCaution { get; }

		[Internal, Field ("NSImageNameStatusAvailable")]
		NSString NSImageNameStatusAvailable { get; }

		[Internal, Field ("NSImageNameStatusPartiallyAvailable")]
		NSString NSImageNameStatusPartiallyAvailable { get; }

		[Internal, Field ("NSImageNameStatusUnavailable")]
		NSString NSImageNameStatusUnavailable { get; }

		[Internal, Field ("NSImageNameStatusNone")]
		NSString NSImageNameStatusNone { get; }

		[Internal, Field ("NSImageNameApplicationIcon")]
		NSString NSImageNameApplicationIcon { get; }

		[Internal, Field ("NSImageNameMenuOnStateTemplate")]
		NSString NSImageNameMenuOnStateTemplate { get; }

		[Internal, Field ("NSImageNameMenuMixedStateTemplate")]
		NSString NSImageNameMenuMixedStateTemplate { get; }

		[Internal, Field ("NSImageNameUserGuest")]
		NSString NSImageNameUserGuest { get; }

		[Internal, Field ("NSImageNameMobileMe")]
		NSString NSImageNameMobileMe { get; }

		[Static, Export("imageWithSize:flipped:drawingHandler:")]
		NSImage ImageWithSize (CGSize size, bool flipped, NSCustomImageRepDrawingHandler drawingHandler);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSImageDelegate {
		[Export ("imageDidNotDraw:inRect:"), DelegateName ("NSImageRect"), DefaultValue (null)]
		NSImage ImageDidNotDraw (NSObject sender, CGRect aRect);

		[Export ("image:willLoadRepresentation:"), EventArgs ("NSImageLoad")]
		void WillLoadRepresentation (NSImage image, NSImageRep rep);

		[Export ("image:didLoadRepresentationHeader:"), EventArgs ("NSImageLoad")]
		void DidLoadRepresentationHeader (NSImage image, NSImageRep rep);

		[Export ("image:didLoadPartOfRepresentation:withValidRows:"), EventArgs ("NSImagePartial")]
		void DidLoadPartOfRepresentation (NSImage image, NSImageRep rep, nint rows);

		[Export ("image:didLoadRepresentation:withStatus:"), EventArgs ("NSImageLoadRepresentation")]
		void DidLoadRepresentation (NSImage image, NSImageRep rep, NSImageLoadStatus status);
	}

	[BaseType (typeof (NSCell))]
	public interface NSImageCell {
		//Detected properties
		[Export ("imageAlignment")]
		NSImageAlignment ImageAlignment { get; set; }

		[Export ("imageScaling")]
		NSImageScale ImageScaling { get; set; }

		[Export ("imageFrameStyle")]
		NSImageFrameStyle ImageFrameStyle { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface NSImageRep {
		[Export ("draw")]
		bool Draw ();

		[Export ("drawAtPoint:")]
		bool DrawAtPoint (CGPoint point);

		[Export ("drawInRect:")]
		bool DrawInRect (CGRect rect);

		[Export ("drawInRect:fromRect:operation:fraction:respectFlipped:hints:")]
		bool DrawInRect (CGRect dstSpacePortionRect, CGRect srcSpacePortionRect, NSCompositingOperation op, nfloat requestedAlpha, bool respectContextIsFlipped, [NullAllowed] NSDictionary hints);

		[Export ("setAlpha:")]
		void SetAlpha (bool alpha);

		[Export ("hasAlpha")]
		bool HasAlpha { get; }

		[Static]
		[Export ("registerImageRepClass:")]
		void RegisterImageRepClass (Class imageRepClass);

		[Static]
		[Export ("unregisterImageRepClass:")]
		void UnregisterImageRepClass (Class imageRepClass);

		//[Static]
		//[Export ("registeredImageRepClasses")]
		//Class [] RegisteredImageRepClasses ();

		[Static]
		[Export ("imageRepClassForFileType:")]
		Class ImageRepClassForFileType (string type);

		[Static]
		[Export ("imageRepClassForPasteboardType:")]
		Class ImageRepClassForPasteboardType (string type);

		[Static]
		[Export ("imageRepClassForType:")]
		Class ImageRepClassForType (string type);

		[Static]
		[Export ("imageRepClassForData:")]
		Class ImageRepClassForData (NSData data);

		[Static]
		[Export ("canInitWithData:")]
		bool CanInitWithData (NSData data);

		[Static]
		[Export ("imageUnfilteredFileTypes")]
		string [] ImageUnfilteredFileTypes { get; }

		[Static]
		[Export ("imageUnfilteredPasteboardTypes")]
		string [] ImageUnfilteredPasteboardTypes { get; }

		[Static]
		[Export ("imageFileTypes")]
		string [] ImageFileTypes { get; }

		[Static]
		[Export ("imagePasteboardTypes")]
		string [] ImagePasteboardTypes { get; }

		[Static]
		[Export ("imageUnfilteredTypes")]
		string []ImageUnfilteredTypes { get; }

		[Static]
		[Export ("imageTypes")]
		string [] ImageTypes { get; }

		[Static]
		[Export ("canInitWithPasteboard:")]
		bool CanInitWithPasteboard (NSPasteboard pasteboard);

		[Static]
		[Export ("imageRepsWithContentsOfFile:")]
		NSImageRep [] ImageRepsFromFile (string filename);

		[Static]
		[Export ("imageRepWithContentsOfFile:")]
		NSImageRep ImageRepFromFile (string filename);

		[Static]
		[Export ("imageRepsWithContentsOfURL:")]
		NSImageRep [] ImageRepsFromUrl (NSUrl url);

		[Static]
		[Export ("imageRepWithContentsOfURL:")]
		NSImageRep ImageRepFromUrl (NSUrl url);

		[Static]
		[Export ("imageRepsWithPasteboard:")]
		NSImageRep [] ImageRepsFromPasteboard (NSPasteboard pasteboard);

		[Static]
		[Export ("imageRepWithPasteboard:")]
		NSImageRep ImageRepFromPasteboard (NSPasteboard pasteboard);

		[Export ("CGImageForProposedRect:context:hints:")]
		CGImage AsCGImage (ref CGRect proposedDestRect, [NullAllowed] NSGraphicsContext context, [NullAllowed] NSDictionary hints);

		//Detected properties
		[Export ("size")]
		CGSize Size { get; set; }

		[Export ("opaque")]
		bool Opaque { [Bind ("isOpaque")]get; set; }

		[Export ("colorSpaceName")]
		string ColorSpaceName { get; set; }

		[Export ("bitsPerSample")]
		nint BitsPerSample { get; set; }

		[Export ("pixelsWide")]
		nint PixelsWide { get; set; }

		[Export ("pixelsHigh")]
		nint PixelsHigh { get; set; }
	}

	[BaseType (typeof (NSControl))]
	public interface NSImageView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		//Detected properties
		[Export ("image")]
		NSImage Image { get; [NullAllowed] set; }

		[Export ("imageAlignment")]
		NSImageAlignment ImageAlignment { get; set; }

		[Export ("imageScaling")]
		NSImageScale ImageScaling { get; set; }

		[Export ("imageFrameStyle")]
		NSImageFrameStyle ImageFrameStyle { get; set; }

		[Export ("editable")]
		bool Editable { [Bind ("isEditable")]get; set; }

		[Export ("animates")]
		bool Animates { get; set; }

		[Export ("allowsCutCopyPaste")]
		bool AllowsCutCopyPaste { get; set; }
	}

	[BaseType (typeof (NSControl), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSMatrixDelegate)})]
	public partial interface NSMatrix {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("initWithFrame:mode:prototype:numberOfRows:numberOfColumns:")]
		IntPtr Constructor (CGRect frameRect, NSMatrixMode aMode, NSCell aCell, nint rowsHigh, nint colsWide);

		[Export ("initWithFrame:mode:cellClass:numberOfRows:numberOfColumns:")]
		IntPtr Constructor (CGRect frameRect, NSMatrixMode aMode, Class factoryId, nint rowsHigh, nint colsWide);

		[Export ("makeCellAtRow:column:")]
		NSCell MakeCell (nint row, nint col);

		[Export ("sendAction:to:forAllCells:")]
		void SendAction (Selector aSelector, NSObject anObject, bool forAllCells);

		[Export ("cells")]
		NSCell [] Cells { get; }

		[Export ("sortUsingSelector:")]
		void Sort (Selector comparator);

		//[Export ("sortUsingFunction:context:")][Internal]
		// We need to define NSCompareFunc as:
		// (nint (*)(id, id, void *))
		//void Sort (NSCompareFunc func, IntPtr context);

		[Export ("selectedCell")]
		NSCell SelectedCell { get; }

		[Export ("selectedCells")]
		NSCell [] SelectedCells { get; }

		[Export ("selectedRow")]
		nint SelectedRow { get; }

		[Export ("selectedColumn")]
		nint SelectedColumn { get; }

		[Export ("setSelectionFrom:to:anchor:highlight:")]
		void SetSelection (nint startPos, nint endPos, nint anchorPos, bool highlight);

		[Export ("deselectSelectedCell")]
		void DeselectSelectedCell ();

		[Export ("deselectAllCells")]
		void DeselectAllCells ();

		[Export ("selectCellAtRow:column:")]
		void SelectCell (nint row, nint column);

		[Export ("selectAll:")]
		void SelectAll (NSObject sender);

		[Export ("selectCellWithTag:")]
		bool SelectCellWithTag (nint tag);

		[Export ("setScrollable:")]
		void SetScrollable (bool flag);

		[Export ("setState:atRow:column:")]
		void SetState (nint state, nint row, nint column);

		[Export ("getNumberOfRows:columns:")]
		void GetRowsAndColumnsCount (out nint rowCount, out nint colCount);

		[Export ("numberOfRows")]
		nint Rows { get; }

		[Export ("numberOfColumns")]
		nint Columns { get; }

		[Export ("cellAtRow:column:")][Internal]
		NSCell CellAtRowColumn (nint row, nint column);

		[Export ("cellFrameAtRow:column:")]
		CGRect CellFrameAtRowColumn (nint row, nint column);

		[Export ("getRow:column:ofCell:")]
		bool GetRowColumn (out nint row, out nint column, NSCell aCell);

		[Export ("getRow:column:forPoint:")]
		bool GetRowColumnForPoint (out nint row, out nint column, CGPoint aPoint);

		[Export ("renewRows:columns:")]
		void RenewRowsColumns (nint newRows, nint newCols);

		[Export ("putCell:atRow:column:")]
		void PutCell (NSCell newCell, nint row, nint column);

		[Export ("addRow")]
		void AddRow ();

		[Export ("addRowWithCells:")]
		void AddRowWithCells (NSCell [] newCells);

		[Export ("insertRow:")]
		void InsertRow (nint row);

		[Export ("insertRow:withCells:")]
		void InsertRow (nint row, NSCell [] newCells);

		[Export ("removeRow:")]
		void RemoveRow (nint row);

		[Export ("addColumn")]
		void AddColumn ();

		[Export ("addColumnWithCells:")]
		void AddColumnWithCells (NSCell [] newCells);

		[Export ("insertColumn:")]
		void InsertColumn (nint column);

		[Export ("insertColumn:withCells:")]
		void InsertColumn (nint column, NSCell [] newCells);

		[Export ("removeColumn:")]
		void RemoveColumn (nint col);

		[Export ("cellWithTag:")]
		NSCell CellWithTag (nint anInt);

		[Export ("sizeToCells")]
		void SizeToCells ();
									       
		[Export ("setValidateSize:")]
		void SetValidateSize (bool flag);

		[Export ("drawCellAtRow:column:")]
		void DrawCellAtRowColumn (nint row, nint column);

		[Export ("highlightCell:atRow:column:")]
		void HighlightCell (bool highlight, nint row, nint column);

		[Export ("scrollCellToVisibleAtRow:column:")]
		void ScrollCellToVisible (nint row, nint column);

		[Export ("mouseDownFlags")]
		nint MouseDownFlags ();

		[Export ("mouseDown:")]
		void MouseDown (NSEvent theEvent);

		[Export ("performKeyEquivalent:")]
		bool PerformKeyEquivalent (NSEvent theEvent);

		[Export ("sendAction")]
		bool SendAction ();

		[Export ("sendDoubleAction")]
		void SendDoubleAction ();

		[Export ("textShouldBeginEditing:")]
		bool ShouldBeginEditing (NSText textObject);

		[Export ("textShouldEndEditing:")]
		bool ShouldEndEditing (NSText textObject);

		[Export ("textDidBeginEditing:")]
		void DidBeginEditing (NSNotification notification);

		[Export ("textDidEndEditing:")]
		void DidEndEditing (NSNotification notification);

		[Export ("textDidChange:")]
		void Changed (NSNotification notification);

		[Export ("selectText:")]
		void SelectText (NSObject sender);

		[Export ("selectTextAtRow:column:")]
		NSObject SelectTextAtRowColumn (nint row, nint column);

		[Export ("acceptsFirstMouse:")]
		bool AcceptsFirstMouse (NSEvent theEvent);

		[Export ("resetCursorRects")]
		void ResetCursorRects ();

		[Export ("setToolTip:forCell:")]
		void SetToolTipForCell (string toolTipString, NSCell cell);

		[Export ("toolTipForCell:")]
		string ToolTipForCell (NSCell cell);

		//Detected properties
		[Export ("cellClass")]
		Class CellClass { get; set; }

		[Export ("prototype")]
		NSCell Prototype { get; set; }

		[Export ("mode")]
		NSMatrixMode Mode { get; set; }

		[Export ("allowsEmptySelection")]
		bool AllowsEmptySelection { get; set; }

		[Export ("selectionByRect")]
		bool SelectionByRect { [Bind ("isSelectionByRect")]get; set; }

		[Export ("cellSize")]
		CGSize CellSize { get; set; }

		[Export ("intercellSpacing")]
		CGSize IntercellSpacing { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("cellBackgroundColor")]
		NSColor CellBackgroundColor { get; set; }

		[Export ("drawsCellBackground")]
		bool DrawsCellBackground { get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("doubleAction")]
		Selector DoubleAction { get; set; }

		[Export ("autosizesCells")]
		bool AutosizesCells { get; set; }

		[Export ("autoscroll")]
		bool Autoscroll { [Bind ("isAutoscroll")]get; set; }

		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSMatrixDelegate Delegate { get; set; }

		//Detected properties
		[Export ("tabKeyTraversesCells")]
		bool TabKeyTraversesCells { get; set; }

		[Export ("keyCell")]
		NSObject KeyCell { get; set; }
	}

	[BaseType (typeof (NSControl))]
	public interface NSLevelIndicator {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("minValue")]
		double MinValue { get; set; }

		[Export ("maxValue")]
		double MaxValue { get; set; }

		[Export ("warningValue")]
		double WarningValue { get; set; }

		[Export ("criticalValue")]
		double CriticalValue { get; set; }

		[Export ("tickMarkPosition")]
		NSTickMarkPosition TickMarkPosition { get; set; }

		[Export ("numberOfTickMarks")]
		nint TickMarkCount { get; set; }

		[Export ("numberOfMajorTickMarks")]
		nint MajorTickMarkCount { get; set; }

		[Export ("tickMarkValueAtIndex:")]
		double TickMarkValueAt (nint index);

		[Export ("rectOfTickMarkAtIndex:")]
		CGRect RectOfTickMark (nint index);
	}

	[BaseType (typeof (NSActionCell))]
	public interface NSLevelIndicatorCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("initWithLevelIndicatorStyle:")]
		IntPtr Constructor (NSLevelIndicatorStyle levelIndicatorStyle);

		[Export ("levelIndicatorStyle")]
		NSLevelIndicatorStyle LevelIndicatorStyle { get; set; }

		[Export ("minValue")]
		double MinValue { get; set; }

		[Export ("maxValue")]
		double MaxValue { get; set; }

		[Export ("warningValue")]
		double WarningValue { get; set; }

		[Export ("criticalValue")]
		double CriticalValue { get; set; }

		[Export ("tickMarkPosition")]
		NSTickMarkPosition TickMarkPosition { get; set; }

		[Export ("numberOfTickMarks")]
		nint TickMarkCount { get; set; }

		[Export ("numberOfMajorTickMarks")]
		nint MajorTickMarkCount { get; set; }

		[Export ("rectOfTickMarkAtIndex:")]
		CGRect RectOfTickMarkAt (nint index);

		[Export ("tickMarkValueAtIndex:")]
		double TickMarkValueAt (nint index);

		[Export ("setImage:")]
		void SetImage (NSImage image);
	}

	[Lion]
	[BaseType (typeof (NSObject))]
	public interface NSLayoutConstraint : NSAnimatablePropertyContainer {
		[Static]
		[Export ("constraintsWithVisualFormat:options:metrics:views:")]
		NSLayoutConstraint [] FromVisualFormat (string format, NSLayoutFormatOptions formatOptions, [NullAllowed] NSDictionary metrics, NSDictionary views);

		[Static]
		[Export ("constraintWithItem:attribute:relatedBy:toItem:attribute:multiplier:constant:")]
		NSLayoutConstraint Create (NSObject view1, NSLayoutAttribute attribute1, NSLayoutRelation relation, [NullAllowed] NSObject view2, NSLayoutAttribute attribute2, nfloat multiplier, nfloat constant);
		
		[Export ("priority")]
		float Priority { get; set;  } // 32-bit

		[Export ("shouldBeArchived")]
		bool ShouldBeArchived { get; set;  }

		[Export ("firstItem")]
		NSObject FirstItem { get;  }

		[Export ("firstAttribute")]
		NSLayoutAttribute FirstAttribute { get;  }

		[Export ("relation")]
		NSLayoutRelation Relation { get;  }

		[Export ("secondItem")]
		NSObject SecondItem { get;  }

		[Export ("secondAttribute")]
		NSLayoutAttribute SecondAttribute { get;  }

		[Export ("multiplier")]
		nfloat Multiplier { get;  }

		[Export ("constant")]
		nfloat Constant { get; set;  }
	}
	
	[BaseType (typeof (NSObject))]
	public partial interface NSLayoutManager {
		[Export ("attributedString")]
		NSAttributedString AttributedString { get; }

		[Export ("replaceTextStorage:")]
		void ReplaceTextStorage (NSTextStorage newTextStorage);

		[Export ("textContainers")]
		NSTextContainer [] TextContainers { get; }

		[Export ("addTextContainer:")]
		[PostGet ("TextContainers")]
		void AddTextContainer (NSTextContainer container);

		[Export ("insertTextContainer:atIndex:")]
		[PostGet ("TextContainers")]
		void InsertTextContainer (NSTextContainer container, nint index);

		[Export ("removeTextContainerAtIndex:")]
		[PostGet ("TextContainers")]
		void RemoveTextContainer (nint index);

		[Export ("textContainerChangedGeometry:")]
		void TextContainerChangedGeometry (NSTextContainer container);

		[Export ("textContainerChangedTextView:")]
		void TextContainerChangedTextView (NSTextContainer container);

		[Export ("layoutOptions")]
		NSGlyphStorageOptions LayoutOptions { get; }

		[Export ("hasNonContiguousLayout")]
		bool HasNonContiguousLayout { get; }

		//[Export ("invalidateGlyphsForCharacterRange:changeInLength:actualCharacterRange:")]
		//void InvalidateGlyphs (NSRange charRange, int changeInLength, NSRangePointer actualCharRange);

		//[Export ("invalidateLayoutForCharacterRange:actualCharacterRange:")]
		//void InvalidateLayout (NSRange charRange, NSRangePointer actualCharRange);

		//[Export ("invalidateLayoutForCharacterRange:isSoft:actualCharacterRange:")]
		//void InvalidateLayout (NSRange charRange, bool isSoft, NSRangePointer actualCharRange);

		[Export ("invalidateDisplayForCharacterRange:")]
		void InvalidateDisplayForCharacterRange (NSRange charRange);

		[Export ("invalidateDisplayForGlyphRange:")]
		void InvalidateDisplayForGlyphRange (NSRange glyphRange);

		[Export ("textStorage:edited:range:changeInLength:invalidatedRange:")]
		void TextStorageEdited (NSTextStorage str, NSTextStorageEditedFlags editedMask, NSRange newCharRange, nint changeInLength, NSRange invalidatedCharRange);

		[Export ("ensureGlyphsForCharacterRange:")]
		void EnsureGlyphsForCharacterRange (NSRange charRange);

		[Export ("ensureGlyphsForGlyphRange:")]
		void EnsureGlyphsForGlyphRange (NSRange glyphRange);

		[Export ("ensureLayoutForCharacterRange:")]
		void EnsureLayoutForCharacterRange (NSRange charRange);

		[Export ("ensureLayoutForGlyphRange:")]
		void EnsureLayoutForGlyphRange (NSRange glyphRange);

		[Export ("ensureLayoutForTextContainer:")]
		void EnsureLayoutForTextContainer (NSTextContainer container);

		[Export ("ensureLayoutForBoundingRect:inTextContainer:")]
		void EnsureLayoutForBoundingRect (CGRect bounds, NSTextContainer container);

		//[Export ("insertGlyphs:length:forStartingGlyphAtIndex:characterIndex:")]
		//void InsertGlyphs (uint [] glyphs, int length, int glyphIndex, int charIndex);

		[Export ("insertGlyph:atGlyphIndex:characterIndex:")]
		void InsertGlyph (nuint glyph, nint glyphIndex, nint charIndex);

		[Export ("replaceGlyphAtIndex:withGlyph:")]
		void ReplaceGlyphAtIndex (nint glyphIndex, nuint newGlyph);

		[Export ("deleteGlyphsInRange:")]
		void DeleteGlyphs (NSRange glyphRange);

		[Export ("setCharacterIndex:forGlyphAtIndex:")]
		void SetCharacterIndex (nint charIndex, nint glyphIndex);

		[Export ("setIntAttribute:value:forGlyphAtIndex:")]
		void SetIntAttribute (nint attributeTag, nint value, nint glyphIndex);

		[Export ("invalidateGlyphsOnLayoutInvalidationForGlyphRange:")]
		void InvalidateGlyphsOnLayoutInvalidation (NSRange glyphRange);

		[Export ("numberOfGlyphs")]
		nint NumberOfGlyphs { get; }

		[Export ("glyphAtIndex:isValidIndex:")]
		nuint GlyphAtIndexisValidIndex (nuint glyphIndex, ref bool isValidIndex);

		[Export ("glyphAtIndex:")]
		nuint GlyphCount (nint glyphIndex);

		[Export ("isValidGlyphIndex:")]
		bool IsValidGlyphIndex (nint glyphIndex);

		[Export ("characterIndexForGlyphAtIndex:")]
		nuint CharacterIndexForGlyphAtIndex (nint glyphIndex);

		[Export ("glyphIndexForCharacterAtIndex:")]
		nuint GlyphIndexForCharacterAtIndex (nint charIndex);

		[Export ("intAttribute:forGlyphAtIndex:")]
		nint IntAttributeforGlyphAtIndex (nint attributeTag, nint glyphIndex);

		// TODO: bind this with a safe version
		[Export ("getGlyphsInRange:glyphs:characterIndexes:glyphInscriptions:elasticBits:"), Internal]
		nint GetGlyphs (NSRange glyphRange, IntPtr glyphBuffer, IntPtr charIndexBuffer, IntPtr inscribeBuffer, IntPtr elasticBuffer);

		// TODO: bind this with a safe version
		[Internal, Export ("getGlyphsInRange:glyphs:characterIndexes:glyphInscriptions:elasticBits:bidiLevels:")]
		nint GetGlyphs (NSRange glyphRange, IntPtr glyphBuffer, IntPtr charIndexBuffer, IntPtr inscribeBuffer, IntPtr elasticBuffer, IntPtr bidiLevelBuffer);

		// TODO: bidn this with a safe version
		[Internal, Export ("getGlyphs:range:")]
		nuint GetGlyphsrange (IntPtr glyphArray, NSRange glyphRange);

		[Export ("setTextContainer:forGlyphRange:")]
		void SetTextContainerForRange (NSTextContainer container, NSRange glyphRange);

		[Export ("setLineFragmentRect:forGlyphRange:usedRect:")]
		void SetLineFragmentRect (CGRect fragmentRect, NSRange glyphRange, CGRect usedRect);

		[Export ("setExtraLineFragmentRect:usedRect:textContainer:")]
		void SetExtraLineFragmentRect (CGRect fragmentRect, CGRect usedRect, NSTextContainer container);

		[Export ("setLocation:forStartOfGlyphRange:")]
		void SetLocation (CGPoint location, NSRange forStartOfGlyphRange);

		//[Export ("setLocations:startingGlyphIndexes:count:forGlyphRange:")]
		//void SetLocations (NSPointArray locations, int glyphIndexes, uint count, NSRange glyphRange);

		[Export ("setNotShownAttribute:forGlyphAtIndex:")]
		void SetNotShownAttribute (bool flag, nint glyphIndex);

		[Export ("setDrawsOutsideLineFragment:forGlyphAtIndex:")]
		void SetDrawsOutsideLineFragment (bool flag, nint glyphIndex);

		[Export ("setAttachmentSize:forGlyphRange:")]
		void SetAttachmentSize (CGSize attachmentSize, NSRange glyphRange);

		[Export ("getFirstUnlaidCharacterIndex:glyphIndex:")]
		void GetFirstUnlaidCharacterIndex (ref nuint charIndex, ref nuint glyphIndex);

		[Export ("firstUnlaidCharacterIndex")]
		nint FirstUnlaidCharacterIndex { get; }

		[Export ("firstUnlaidGlyphIndex")]
		nint FirstUnlaidGlyphIndex { get; }

		//[Export ("textContainerForGlyphAtIndex:effectiveRange:")]
		//NSTextContainer TextContainerForGlyphAt (int glyphIndex, NSRangePointer effectiveGlyphRange);

		[Export ("usedRectForTextContainer:")]
		CGRect GetUsedRectForTextContainer (NSTextContainer container);

		//[Export ("lineFragmentRectForGlyphAtIndex:effectiveRange:")]
		//CGRect LineFragmentRectForGlyphAt (int glyphIndex, NSRangePointer effectiveGlyphRange);

		//[Export ("lineFragmentUsedRectForGlyphAtIndex:effectiveRange:")]
		//CGRect LineFragmentUsedRectForGlyphAt (int glyphIndex, NSRangePointer effectiveGlyphRange);

		//[Export ("lineFragmentRectForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		//CGRect LineFragmentRectForGlyphAt (int glyphIndex, NSRangePointer effectiveGlyphRange, bool flag);

		//[Export ("lineFragmentUsedRectForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		//CGRect LineFragmentUsedRectForGlyphAt (int glyphIndex, NSRangePointer effectiveGlyphRange, bool flag);

		//[Export ("textContainerForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		//NSTextContainer TextContainerForGlyphAt (int glyphIndex, NSRangePointer effectiveGlyphRange, bool flag);

		[Export ("extraLineFragmentRect")]
		CGRect ExtraLineFragmentRect { get; }

		[Export ("extraLineFragmentUsedRect")]
		CGRect ExtraLineFragmentUsedRect { get; }

		[Export ("extraLineFragmentTextContainer")]
		NSTextContainer ExtraLineFragmentTextContainer { get; }

		[Export ("locationForGlyphAtIndex:")]
		CGPoint LocationForGlyphAtIndex (nint glyphIndex);

		[Export ("notShownAttributeForGlyphAtIndex:")]
		bool NotShownAttributeForGlyphAtIndex (nint glyphIndex);

		[Export ("drawsOutsideLineFragmentForGlyphAtIndex:")]
		bool DrawsOutsideLineFragmentForGlyphAt (nint glyphIndex);

		[Export ("attachmentSizeForGlyphAtIndex:")]
		CGSize AttachmentSizeForGlyphAt (nint glyphIndex);

		[Export ("setLayoutRect:forTextBlock:glyphRange:")]
		void SetLayoutRect (CGRect layoutRect, NSTextBlock forTextBlock, NSRange glyphRange);

		[Export ("setBoundsRect:forTextBlock:glyphRange:")]
		void SetBoundsRect (CGRect boundsRect, NSTextBlock forTextBlock, NSRange glyphRange);

		[Export ("layoutRectForTextBlock:glyphRange:")]
		CGRect LayoutRect (NSTextBlock block, NSRange glyphRange);

		[Export ("boundsRectForTextBlock:glyphRange:")]
		CGRect BoundsRect (NSTextBlock block, NSRange glyphRange);

		//[Export ("layoutRectForTextBlock:atIndex:effectiveRange:")]
		//CGRect LayoutRect (NSTextBlock block, int glyphIndex, NSRangePointer effectiveGlyphRange);

		//[Export ("boundsRectForTextBlock:atIndex:effectiveRange:")]
		//CGRect BoundsRect (NSTextBlock block, int glyphIndex, NSRangePointer effectiveGlyphRange);

		//[Export ("glyphRangeForCharacterRange:actualCharacterRange:")]
		//NSRange GetGlyphRange (NSRange charRange, NSRangePointer actualCharRange);

		//[Export ("characterRangeForGlyphRange:actualGlyphRange:")]
		//NSRange GetCharacterRange (NSRange glyphRange, NSRangePointer actualGlyphRange);

		[Export ("glyphRangeForTextContainer:")]
		NSRange GetGlyphRange (NSTextContainer container);

		[Export ("rangeOfNominallySpacedGlyphsContainingIndex:")]
		NSRange RangeOfNominallySpacedGlyphsContainingIndex (nint glyphIndex);

		//[Export ("rectArrayForCharacterRange:withinSelectedCharacterRange:inTextContainer:rectCount:")]
		//NSRectArray RectArrayForCharacterRangewithinSelectedCharacterRangeinTextContainerrectCount (NSRange charRange, NSRange selCharRange, NSTextContainer container, uint rectCount);

		//[Export ("rectArrayForGlyphRange:withinSelectedGlyphRange:inTextContainer:rectCount:")]
		//NSRectArray RectArrayForGlyphRangewithinSelectedGlyphRangeinTextContainerrectCount (NSRange glyphRange, NSRange selGlyphRange, NSTextContainer container, uint rectCount);

		[Export ("boundingRectForGlyphRange:inTextContainer:")]
		CGRect BoundingRectForGlyphRange (NSRange glyphRange, NSTextContainer container);

		[Export ("glyphRangeForBoundingRect:inTextContainer:")]
		NSRange GlyphRangeForBoundingRect (CGRect bounds, NSTextContainer container);

		[Export ("glyphRangeForBoundingRectWithoutAdditionalLayout:inTextContainer:")]
		NSRange GlyphRangeForBoundingRectWithoutAdditionalLayout (CGRect bounds, NSTextContainer container);

		[Export ("glyphIndexForPoint:inTextContainer:fractionOfDistanceThroughGlyph:")]
		nuint GlyphIndexForPointInTextContainer (CGPoint point, NSTextContainer container, ref nfloat fractionOfDistanceThroughGlyph);

		[Export ("glyphIndexForPoint:inTextContainer:")]
		nuint GlyphIndexForPoint (CGPoint point, NSTextContainer container);

		[Export ("fractionOfDistanceThroughGlyphForPoint:inTextContainer:")]
		nfloat FractionOfDistanceThroughGlyphForPoint (CGPoint point, NSTextContainer container);

		[Export ("characterIndexForPoint:inTextContainer:fractionOfDistanceBetweenInsertionPoints:")]
		nuint CharacterIndexForPoint (CGPoint point, NSTextContainer container, ref nfloat fractionOfDistanceBetweenInsertionPoints);

		[Export ("getLineFragmentInsertionPointsForCharacterAtIndex:alternatePositions:inDisplayOrder:positions:characterIndexes:")]
		nuint GetLineFragmentInsertionPoints (nuint charIndex, bool aFlag, bool dFlag, IntPtr positions, IntPtr charIndexes);

		//[Export ("temporaryAttributesAtCharacterIndex:effectiveRange:")]
		//NSDictionary GetTemporaryAttributes (int charIndex, NSRangePointer effectiveCharRange);

		[Export ("setTemporaryAttributes:forCharacterRange:")]
		void SetTemporaryAttributes (NSDictionary attrs, NSRange charRange);

		[Export ("addTemporaryAttributes:forCharacterRange:")]
		void AddTemporaryAttributes (NSDictionary attrs, NSRange charRange);

		[Export ("removeTemporaryAttribute:forCharacterRange:")]
		void RemoveTemporaryAttribute (string attrName, NSRange charRange);

		//[Export ("temporaryAttribute:atCharacterIndex:effectiveRange:")]
		//NSObject GetTemporaryAttribute (string attrName, uint location, NSRangePointer range);

		//[Export ("temporaryAttribute:atCharacterIndex:longestEffectiveRange:inRange:")]
		//NSObject GetTemporaryAttribute (string attrName, uint location, NSRangePointer range, NSRange rangeLimit);

		//[Export ("temporaryAttributesAtCharacterIndex:longestEffectiveRange:inRange:")]
		//NSDictionary GetTemporaryAttributes (int characterIndex, NSRangePointer longestEffectiveRange, NSRange rangeLimit);

		[Export ("addTemporaryAttribute:value:forCharacterRange:")]
		void AddTemporaryAttribute (string attrName, NSObject value, NSRange charRange);

		[Export ("substituteFontForFont:")]
		NSFont SubstituteFontForFont (NSFont originalFont);

		[Export ("defaultLineHeightForFont:")]
		nfloat DefaultLineHeightForFont (NSFont theFont);

		[Export ("defaultBaselineOffsetForFont:")]
		nfloat DefaultBaselineOffsetForFont (NSFont theFont);

		//Detected properties
		[Export ("textStorage")]
		NSTextStorage TextStorage { get; set; }

		[Export ("glyphGenerator")]
		NSGlyphGenerator GlyphGenerator { get; set; }

		[Export ("typesetter")]
		NSTypesetter Typesetter { get; set; }

		[Export ("delegate")]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSLayoutManagerDelegate Delegate { get; set; }

		[Export ("backgroundLayoutEnabled")]
		bool BackgroundLayoutEnabled { get; set; }

		[Export ("usesScreenFonts")]
		bool UsesScreenFonts { get; set; }

		[Export ("showsInvisibleCharacters")]
		bool ShowsInvisibleCharacters { get; set; }

		[Export ("showsControlCharacters")]
		bool ShowsControlCharacters { get; set; }

		[Export ("hyphenationFactor")]
		float HyphenationFactor { get; set; } // 32-bit

		[Export ("defaultAttachmentScaling")]
		NSImageScaling DefaultAttachmentScaling { get; set; }

		[Export ("typesetterBehavior")]
		NSTypesetterBehavior TypesetterBehavior { get; set; }

		[Export ("allowsNonContiguousLayout")]
		bool AllowsNonContiguousLayout { get; set; }

		[Export ("usesFontLeading")]
		bool UsesFontLeading { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSLayoutManagerDelegate {
		[Export ("layoutManagerDidInvalidateLayout:")]
		void LayoutInvalidated (NSLayoutManager sender);

		[Export ("layoutManager:didCompleteLayoutForTextContainer:atEnd:")]
		void LayoutCompleted (NSLayoutManager layoutManager, NSTextContainer textContainer, bool layoutFinishedFlag);

		[Export ("layoutManager:shouldUseTemporaryAttributes:forDrawingToScreen:atCharacterIndex:effectiveRange:")]
		NSDictionary ShouldUseTemporaryAttributes (NSLayoutManager layoutManager, NSDictionary temporaryAttributes, bool drawingToScreen, nint charIndex, IntPtr effectiveCharRange);

	}

	//[Mac (10,10)]
	[BaseType (typeof (NSGestureRecognizer))]
	public interface NSMagnificationGestureRecognizer {
		[Export ("initWithTarget:action:")]
		IntPtr Constructor (NSObject target, Selector action);

		[Export ("magnification")]
		nfloat Magnification { get; set; }
	}


	[Model]
	[BaseType (typeof (NSObject))]
	public interface NSMatrixDelegate {
		[Export ("control:textShouldBeginEditing:"), DelegateName ("NSControlText"), DefaultValue (true)]
		bool TextShouldBeginEditing (NSControl control, NSText fieldEditor);

		[Export ("control:textShouldEndEditing:"), DelegateName ("NSControlText"), DefaultValue (true)]
		bool TextShouldEndEditing (NSControl control, NSText fieldEditor);

		[Export ("control:didFailToFormatString:errorDescription:"), DelegateName ("NSControlTextError"), DefaultValue (true)]
		bool DidFailToFormatString (NSControl control, string str, string error);
		
		[Export ("control:didFailToValidatePartialString:errorDescription:"), EventArgs ("NSControlTextError")]
		void DidFailToValidatePartialString (NSControl control, string str, string error);
		
		[Export ("control:isValidObject:"), DelegateName ("NSControlTextValidation"), DefaultValue (true)]
		bool IsValidObject (NSControl control, NSObject objectToValidate);

		[Export ("control:textView:doCommandBySelector:"), DelegateName ("NSControlCommand"), DefaultValue (false)]
		bool DoCommandBySelector (NSControl control, NSTextView textView, Selector commandSelector);

		[Export ("control:textView:completions:forPartialWordRange:indexOfSelectedItem:"), DelegateName ("NSControlTextCompletion"), DefaultValue (null)]
		string [] GetCompletions (NSControl control, NSTextView textView, string [] words, NSRange charRange, nint index);
	}

	[BaseType (typeof (NSObject))]
	[Dispose ("__mt_accessory_var = null;")]
	public interface NSPageLayout {
		[Static]
		[Export ("pageLayout")]
		NSPageLayout PageLayout { get; }

		[Export ("addAccessoryController:")]
		[PostSnippet ("__mt_accessory_var = AccessoryControllers();")]
		void AddAccessoryController (NSViewController accessoryController);

		[Export ("removeAccessoryController:")]
		[PostSnippet ("__mt_accessory_var = AccessoryControllers();")]
		void RemoveAccessoryController (NSViewController accessoryController);

		[Export ("accessoryControllers")]
		NSViewController [] AccessoryControllers ();

		[Export ("beginSheetWithPrintInfo:modalForWindow:delegate:didEndSelector:contextInfo:")]
		void BeginSheet (NSPrintInfo printInfo, NSWindow docWindow, [NullAllowed] NSObject del, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);

		[Export ("runModalWithPrintInfo:")]
		nint RunModalWithPrintInfo (NSPrintInfo printInfo);

		[Export ("runModal")]
		nint RunModal ();

		[Export ("printInfo")]
		NSPrintInfo PrintInfo { get; }
	}

	[BaseType (typeof (NSWindow))]
	public interface NSPanel {
		//Detected properties
		[Export ("floatingPanel")]
		bool FloatingPanel { [Bind ("isFloatingPanel")]get; set; }

		[Export ("becomesKeyOnlyIfNeeded")]
		bool BecomesKeyOnlyIfNeeded { get; set; }

		[Export ("worksWhenModal")]
		bool WorksWhenModal { get; set; }

		[Export ("initWithContentRect:styleMask:backing:defer:")]
		IntPtr Constructor (CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation);
	}

	[BaseType (typeof (NSObject))]
	public interface NSParagraphStyle {
		[Static]
		[Export ("defaultParagraphStyle")]
		NSParagraphStyle DefaultParagraphStyle { get; [NotImplemented] set; }

		[Static]
		[Export ("defaultWritingDirectionForLanguage:")]
		NSWritingDirection DefaultWritingDirection (string languageName);

		[Export ("lineSpacing")]
		nfloat LineSpacing { get; [NotImplemented] set; }

		[Export ("paragraphSpacing")]
		nfloat ParagraphSpacing { get; [NotImplemented] set; }

		[Export ("alignment")]
		NSTextAlignment Alignment { get; [NotImplemented] set; }

		[Export ("headIndent")]
		nfloat HeadIndent { get; [NotImplemented] set; }

		[Export ("tailIndent")]
		nfloat TailIndent { get; [NotImplemented] set; }

		[Export ("firstLineHeadIndent")]
		nfloat FirstLineHeadIndent { get; [NotImplemented] set; }

		[Export ("tabStops")]
		NSTextTab [] TabStops { get; [NotImplemented] set; }

		[Export ("minimumLineHeight")]
		nfloat MinimumLineHeight { get; [NotImplemented] set; }

		[Export ("maximumLineHeight")]
		nfloat MaximumLineHeight { get; [NotImplemented] set; }

		[Export ("lineBreakMode")]
		NSLineBreakMode LineBreakMode { get; [NotImplemented] set; }

		[Export ("baseWritingDirection")]
		NSWritingDirection BaseWritingDirection { get; [NotImplemented] set; }

		[Export ("lineHeightMultiple")]
		nfloat LineHeightMultiple { get; [NotImplemented] set; }

		[Export ("paragraphSpacingBefore")]
		nfloat ParagraphSpacingBefore { get; [NotImplemented] set; }

		[Export ("defaultTabInterval")]
		nfloat DefaultTabInterval { get; [NotImplemented] set; }

		[Export ("textBlocks")]
		NSTextTableBlock [] TextBlocks { get; [NotImplemented] set; }

		[Export ("textLists")]
		NSTextList[] TextLists { get; [NotImplemented] set; }

		[Export ("hyphenationFactor")]
		float HyphenationFactor { get; [NotImplemented] set; } // 32-bit

		[Export ("tighteningFactorForTruncation")]
		float TighteningFactorForTruncation { get; [NotImplemented] set; } // 32-bit

		[Export ("headerLevel")]
		nint HeaderLevel { get; [NotImplemented] set; }
	}

	[BaseType (typeof (NSParagraphStyle))]
	public interface NSMutableParagraphStyle {

		[Export ("addTabStop:")]
		[PostGet ("TabStops")]
		void AddTabStop (NSTextTab anObject);

		[Export ("removeTabStop:")]
		[PostGet ("TabStops")]
		void RemoveTabStop (NSTextTab anObject);

		[Export ("tabStops")]
		[Override]
		NSTextTab [] TabStops { get; set; }

		[Export ("setParagraphStyle:")]
		void SetParagraphStyle (NSParagraphStyle obj);

		[Export ("defaultTabInterval")]
		[Override]
		nfloat DefaultTabInterval { get; set; }

		[Export ("setTextBlocks:")]
		void SetTextBlocks (NSTextBlock [] array);

		[Export ("setTextLists:")]
		void SetTextLists (NSTextList [] array);

		[Export ("tighteningFactorForTruncation")]
		[Override]
		float TighteningFactorForTruncation { get; set; } // 32-bit

		[Export ("headerLevel")]
		[Override]
		nint HeaderLevel { get; set; }

		[Export ("lineSpacing")]
		[Override]
		nfloat LineSpacing { get; set; }

		[Export ("alignment")]
		[Override]
		NSTextAlignment Alignment { get; set; }

		[Export ("headIndent")]
		[Override]
		nfloat HeadIndent { get; set; }

		[Export ("tailIndent")]
		[Override]
		nfloat TailIndent { get; set; }

		[Export ("firstLineHeadIndent")]
		[Override]
		nfloat FirstLineHeadIndent { get; set; }

		[Export ("minimumLineHeight")]
		[Override]
		nfloat MinimumLineHeight { get; set; }

		[Export ("maximumLineHeight")]
		[Override]
		nfloat MaximumLineHeight { get; set; }

		[Export ("lineBreakMode")]
		[Override]
		NSLineBreakMode LineBreakMode { get; set; }

		[Export ("baseWritingDirection")]
		[Override]
		NSWritingDirection BaseWritingDirection { get; set; }

		[Export ("lineHeightMultiple")]
		[Override]
		nfloat LineHeightMultiple { get; set; }

		[Export ("paragraphSpacing")]
		[Override]
		nfloat ParagraphSpacing { get; set; }

		[Export ("paragraphSpacingBefore")]
		[Override]
		nfloat ParagraphSpacingBefore { get; set; }

		[Export ("hyphenationFactor")]
		[Override]
		float HyphenationFactor { get; set; } // 32-bit
	}

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // An uncaught exception was raised: +[NSPasteboard alloc]: unrecognized selector sent to class 0xac3dcbf0
	public partial interface NSPasteboard {
		[Static]
		[Export ("generalPasteboard")]
		NSPasteboard GeneralPasteboard { get; }

		[Static]
		[Export ("pasteboardWithName:")]
		NSPasteboard FromName (string name);

		[Static]
		[Export ("pasteboardWithUniqueName")]
		NSPasteboard CreateWithUniqueName ();

		[Export ("name")]
		string Name { get; }

		[Export ("changeCount")]
		nint ChangeCount { get; }

		[Export ("releaseGlobally")]
		void ReleaseGlobally ();

		[Export ("clearContents")]
		nint ClearContents ();

		[Export ("writeObjects:")]
		bool WriteObjects (NSObject [] objects);

		[Export ("readObjectsForClasses:options:")]
		NSObject [] ReadObjectsForClasses (Class [] classArray, [NullAllowed] NSDictionary options);

		[Export ("pasteboardItems")]
		NSPasteboardItem [] PasteboardItems { get; }

		[Export ("indexOfPasteboardItem:")]
		nuint IndexOf (NSPasteboardItem pasteboardItem);

		[Export ("canReadItemWithDataConformingToTypes:")]
		bool CanReadItemWithDataConformingToTypes (string [] utiTypes);

		[Export ("canReadObjectForClasses:options:")]
		bool CanReadObjectForClasses (Class [] classArray, [NullAllowed] NSDictionary options);

		[Export ("declareTypes:owner:")]
		nint DeclareTypes (string [] newTypes, [NullAllowed] NSObject newOwner);

		[Export ("addTypes:owner:")]
		nint AddTypes (string [] newTypes, [NullAllowed] NSObject newOwner);

		[Export ("types")]
		string [] Types { get; }

		[Export ("availableTypeFromArray:")]
		string GetAvailableTypeFromArray (string [] types);

		[Export ("setData:forType:")]
		bool SetDataForType (NSData data, string dataType);

		[Export ("setPropertyList:forType:")]
		bool SetPropertyListForType (NSObject plist, string dataType);

		[Export ("setString:forType:")]
		bool SetStringForType (string str, string dataType);

		[Export ("dataForType:")]
		NSData GetDataForType (string dataType);

		[Export ("propertyListForType:")]
		NSObject GetPropertyListForType (string dataType);

		[Export ("stringForType:")]
		string GetStringForType (string dataType);

		// Pasteboard data types for 10.5 (deprecated)

		[Field ("NSStringPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSStringType{ get; }
		
		[Field ("NSFilenamesPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSFilenamesType{ get; }
		
		[Field ("NSPostScriptPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSPostScriptType{ get; }
        
		[Field ("NSTIFFPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSTiffType{ get; }
		
		[Field ("NSRTFPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSRtfType{ get; }
		
		[Field ("NSTabularTextPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSTabularTextType{ get; }
		
		[Field ("NSFontPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSFontType{ get; }
		
		[Field ("NSRulerPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSRulerType{ get; }
		
		[Field ("NSFileContentsPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSFileContentsType{ get; }
		
		[Field ("NSColorPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSColorType{ get; }
		
		[Field ("NSRTFDPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSRtfdType{ get; }
		
		[Field ("NSHTMLPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSHtmlType{ get; }
		
		[Field ("NSPICTPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSPictType{ get; }
		
		[Field ("NSURLPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSUrlType{ get; }
		
		[Field ("NSPDFPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSPdfType{ get; }
		
		[Field ("NSVCardPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSVCardType{ get; }
		
		[Field ("NSFilesPromisePboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSFilesPromiseType{ get; }
		
		[Field ("NSMultipleTextSelectionPboardType")]
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		NSString NSMultipleTextSelectionType{ get; }

		// Pasteboard data types 10.6+
		[Field ("NSPasteboardTypeString")]
		[Introduced (PlatformName.MacOSX, 10, 6)]
		NSString NSPasteboardTypeString { get; }
		
		[Field ("NSPasteboardTypePDF")]
		[Introduced (PlatformName.MacOSX, 10, 6)]
		NSString NSPasteboardTypePDF { get; }
		
		[Field ("NSPasteboardTypeTIFF")]
		[Introduced (PlatformName.MacOSX, 10, 6)]
		NSString NSPasteboardTypeTIFF { get; }
		
		[Field ("NSPasteboardTypePNG")]
		[Introduced (PlatformName.MacOSX, 10, 6)]
		NSString NSPasteboardTypePNG { get; }
		
		[Field ("NSPasteboardTypeRTF")]
		[Introduced (PlatformName.MacOSX, 10, 6)]
		NSString NSPasteboardTypeRTF { get; }
		
		[Field ("NSPasteboardTypeRTFD")]
		[Introduced (PlatformName.MacOSX, 10, 6)]
		NSString NSPasteboardTypeRTFD { get; }

		[Field ("NSPasteboardTypeHTML")]
		[Introduced (PlatformName.MacOSX, 10, 6)]
		NSString NSPasteboardTypeHTML { get; }

		[Field ("NSPasteboardTypeTabularText")]
		[Introduced (PlatformName.MacOSX, 10, 6)]
		NSString NSPasteboardTypeTabularText { get; }

		[Field ("NSPasteboardTypeFont")]
		[Introduced (PlatformName.MacOSX, 10, 6)]
		NSString NSPasteboardTypeFont { get; }

		[Field ("NSPasteboardTypeRuler")]
		[Introduced (PlatformName.MacOSX, 10, 6)]
		NSString NSPasteboardTypeRuler { get; }

		[Field ("NSPasteboardTypeColor")]
		[Introduced (PlatformName.MacOSX, 10, 6)]
		NSString NSPasteboardTypeColor { get; }

		[Field ("NSPasteboardTypeSound")]
		[Introduced (PlatformName.MacOSX, 10, 6)]
		NSString NSPasteboardTypeSound { get; }

		[Field ("NSPasteboardTypeMultipleTextSelection")]
		[Introduced (PlatformName.MacOSX, 10, 6)]
		NSString NSPasteboardTypeMultipleTextSelection { get; }

		[Field ("NSPasteboardTypeFindPanelSearchOptions")]
		[Introduced (PlatformName.MacOSX, 10, 6)]
		NSString NSPasteboardTypeFindPanelSearchOptions { get; }
		
		[Field ("NSPasteboardTypeTextFinderOptions")]
		NSString PasteboardTypeTextFinderOptions { get; }

		[Mac (10, 13)]
		[Field ("NSPasteboardTypeURL")]
		NSString NSPasteboardTypeUrl { get; }

		[Mac (10, 13)]
		[Field ("NSPasteboardTypeFileURL")]
		NSString NSPasteboardTypeFileUrl { get; }
		

		// Pasteboard names: for NSPasteboard.FromName()

		[Field ("NSGeneralPboard")]
		NSString NSGeneralPasteboardName { get; }

		[Field ("NSFontPboard")]
		NSString NSFontPasteboardName { get; }

		[Field ("NSRulerPboard")]
		NSString NSRulerPasteboardName { get; }

		[Field ("NSFindPboard")]
		NSString NSFindPasteboardName { get; }

		[Field ("NSDragPboard")]
		NSString NSDragPasteboardName { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSPasteboardWriting {
		[Export ("writableTypesForPasteboard:")]
		string [] GetWritableTypesForPasteboard (NSPasteboard pasteboard);

		[Export ("writingOptionsForType:pasteboard:")]
		NSPasteboardWritingOptions GetWritingOptionsForType (string type, NSPasteboard pasteboard);

		[Export ("pasteboardPropertyListForType:")]
		NSObject GetPasteboardPropertyListForType (string type);
	}

	[BaseType (typeof (NSObject))]
	public interface NSPasteboardItem {
		[Export ("types")]
		string [] Types { get; }

		[Export ("availableTypeFromArray:")]
		string GetAvailableTypeFromArray (string [] types);

		[Export ("setDataProvider:forTypes:")]
		bool SetDataProviderForTypes (NSPasteboardItemDataProvider dataProvider, string [] types);

		[Export ("setData:forType:")]
		bool SetDataForType (NSData data, string type);

		[Export ("setString:forType:")]
		bool SetStringForType (string str, string type);

		[Export ("setPropertyList:forType:")]
		bool SetPropertyListForType (NSObject propertyList, string type);

		[Export ("dataForType:")]
		NSData GetDataForType (string type);

		[Export ("stringForType:")]
		string GetStringForType (string type);

		[Export ("propertyListForType:")]
		NSObject GetPropertyListForType (string type);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSPasteboardItemDataProvider {
		[Abstract]
		[Export ("pasteboard:item:provideDataForType:")]
		void ProvideDataForType (NSPasteboard pasteboard, NSPasteboardItem item, string type);

		[Abstract]
		[Export ("pasteboardFinishedWithDataProvider:")]
		void FinishedWithDataProvider (NSPasteboard pasteboard);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSPasteboardReading {
		[Abstract]
		[Export ("readableTypesForPasteboard:")]
		string [] GetReadableTypesForPasteboard (NSPasteboard pasteboard);

		[Abstract]
		[Export ("readingOptionsForType:pasteboard:")]
		NSPasteboardReadingOptions GetReadingOptionsForType (string type, NSPasteboard pasteboard);

		[Abstract]
		[Export ("initWithPasteboardPropertyList:ofType:")]
		NSObject InitWithPasteboardPropertyList (NSObject propertyList, string type);
	}
	
	[BaseType (typeof (NSActionCell), Events=new Type [] { typeof (NSPathCellDelegate) }, Delegates=new string [] { "WeakDelegate" })]
	public interface NSPathCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("pathStyle")]
		NSPathStyle PathStyle { get; set; }

		[Export ("URL")]
		NSUrl Url { get; set; }

		[Export ("setObjectValue:")]
		void SetObjectValue (NSObject obj);

		[Export ("allowedTypes")]
		string [] AllowedTypes { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSPathCellDelegate Delegate { get; set; }

		[Static, Export ("pathComponentCellClass")]
		Class PathComponentCellClass { get; }

		[Export ("pathComponentCells")]
		NSPathComponentCell [] PathComponentCells { get; set; }

		[Export ("rectOfPathComponentCell:withFrame:inView:")]
		CGRect GetRect (NSPathComponentCell componentCell, CGRect withFrame, NSView inView);

		[Export ("pathComponentCellAtPoint:withFrame:inView:")]
		NSPathComponentCell GetPathComponent (CGPoint point, CGRect frame, NSView view);

		[Export ("clickedPathComponentCell")]
		NSPathComponentCell ClickedPathComponentCell { get; }

		[Export ("mouseEntered:withFrame:inView:")]
		void MouseEntered (NSEvent evt, CGRect frame, NSView view);

		[Export ("mouseExited:withFrame:inView:")]
		void MouseExited (NSEvent evt, CGRect frame, NSView view);

		[Export ("doubleAction")]
		Selector DoubleAction { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("placeholderString")]
		string PlaceholderString { get; set; }

		[Export ("placeholderAttributedString")]
		NSAttributedString PlaceholderAttributedString { get; set; }

		[Export ("setControlSize:")]
		void SetControlSize (NSControlSize size);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSPathCellDelegate {
		[Export ("pathCell:willDisplayOpenPanel:"), EventArgs ("NSPathCellDisplayPanel")]
		void WillDisplayOpenPanel (NSPathCell pathCell, NSOpenPanel openPanel);

		[Export ("pathCell:willPopUpMenu:"), EventArgs ("NSPathCellMenu")]
		void WillPopupMenu (NSPathCell pathCell, NSMenu menu);
	}

	[BaseType (typeof (NSTextFieldCell))]
	public interface NSPathComponentCell {
		[Export ("image")]
		NSImage Image { get; set; }

		[Export ("URL")]
		NSUrl Url { get; set; }
	}


	[BaseType (typeof (NSControl))]
	public interface NSPathControl {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("URL")]
		NSUrl Url { get; set; }

		[Export ("clickedPathComponentCell")]
		NSPathComponentCell ClickedPathComponentCell { get; }

		[Export ("setDraggingSourceOperationMask:forLocal:")]
		void SetDraggingSource (NSDragOperation operationMask, bool isLocal);

		[Export ("doubleAction")]
		Selector DoubleAction { get; set; }

		[Export ("pathStyle")]
		NSPathStyle PathStyle { get; set; }

		[Export ("pathComponentCells")]
		NSPathComponentCell [] PathComponentCells { get; set; }

		[Export ("backgroundColor"), NullAllowed]
		NSColor BackgroundColor { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }
		[Wrap ("WeakDelegate")]
		NSPathControlDelegate Delegate { get; set; }

		[Export ("menu")]
		NSMenu Menu { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSPathControlDelegate {
		[Abstract]
		[Export ("pathControl:shouldDragPathComponentCell:withPasteboard:")]
		bool ShouldDragPathComponentCell (NSPathControl pathControl, NSPathComponentCell pathComponentCell, NSPasteboard pasteboard);

		[Abstract]
		[Export ("pathControl:validateDrop:")]
		NSDragOperation ValidateDrop (NSPathControl pathControl, NSDraggingInfo info);

		[Abstract]
		[Export ("pathControl:acceptDrop:")]
		bool AcceptDrop (NSPathControl pathControl, NSDraggingInfo info);

		[Abstract]
		[Export ("pathControl:willDisplayOpenPanel:")]
		void WillDisplayOpenPanel (NSPathControl pathControl, NSOpenPanel openPanel);

		[Abstract]
		[Export ("pathControl:willPopUpMenu:")]
		void WillPopUpMenu (NSPathControl pathControl, NSMenu menu);
	}

	[BaseType (typeof (NSResponder))]
	interface NSPopover : NSAppearanceCustomization {
		// [Export ("appearance")]
		// new NSPopoverAppearance Appearance { get; set;  }

		[Export ("behavior")]
		NSPopoverBehavior Behavior { get; set;  }

		[Export ("animates")]
		bool Animates { get; set;  }

		[Export ("contentViewController")]
		NSViewController ContentViewController { get; set;  }

		[Export ("contentSize")]
		CGSize ContentSize { get; set;  }

		[Export ("shown")]
		bool Shown { [Bind ("isShown")] get;  }

		[Export ("positioningRect")]
		CGRect PositioningRect { get; set;  }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSPopoverDelegate Delegate { set; get; }
		
		[Export ("showRelativeToRect:ofView:preferredEdge:")]
		void Show (CGRect relativePositioningRect, NSView positioningView, NSRectEdge preferredEdge);

		[Export ("performClose:")]
		void PerformClose (NSObject sender);

		[Export ("close")]
		void Close ();

		[Field ("NSPopoverCloseReasonKey")]
		NSString CloseReasonKey { get; }
		
		[Field ("NSPopoverCloseReasonStandard")]
		NSString CloseReasonStandard { get; }
		
		[Field ("NSPopoverCloseReasonDetachToWindow")]
		NSString CloseReasonDetachToWindow { get; }
		
		[Notification, Field ("NSPopoverWillShowNotification")]
		NSString WillShowNotification { get; }
		
		[Notification, Field ("NSPopoverDidShowNotification")]
		NSString DidShowNotification { get; }
		
		[Notification (typeof (NSPopoverCloseEventArgs)), Field ("NSPopoverWillCloseNotification")]
		NSString WillCloseNotification { get; }
		
		[Notification (typeof (NSPopoverCloseEventArgs)), Field ("NSPopoverDidCloseNotification")]
		NSString DidCloseNotification { get; }
	}

	public partial interface NSPopoverCloseEventArgs {
		[Internal, Export ("NSPopoverCloseReasonKey")]
		NSString _Reason { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSPopoverDelegate {
		[Export ("popoverShouldClose:")]
		bool ShouldClose (NSPopover popover);

		[Export ("detachableWindowForPopover:")]
		NSWindow GetDetachableWindowForPopover (NSPopover popover);

		[Export ("popoverWillShow:")]
		void WillShow (NSNotification notification);

		[Export ("popoverDidShow:")]
		void DidShow (NSNotification notification);

		[Export ("popoverWillClose:")]
		void WillClose (NSNotification notification);

		[Export ("popoverDidClose:")]
		void DidClose (NSNotification notification);
	}

	[BaseType (typeof (NSButton))]
	public partial interface NSPopUpButton {
		[Export ("initWithFrame:pullsDown:")]
		IntPtr Constructor (CGRect buttonFrame, bool pullsDown);

		[Export ("addItemWithTitle:")]
		void AddItem (string title);

		[Export ("addItemsWithTitles:")]
		void AddItems (string [] itemTitles);

		[Export ("insertItemWithTitle:atIndex:")]
		void InsertItem (string title, nint index);

		[Export ("removeItemWithTitle:")]
		void RemoveItem (string title);

		[Export ("removeItemAtIndex:")]
		void RemoveItem (nint index);

		[Export ("removeAllItems")]
		void RemoveAllItems ();

		[Export ("itemArray")]
		NSMenuItem [] Items ();

		[Export ("numberOfItems")]
		nint ItemCount { get; }

		[Export ("indexOfItem:")]
		nint IndexOfItem (NSMenuItem item);

		[Export ("indexOfItemWithTitle:")]
		nint IndexOfItem (string title);

		[Export ("indexOfItemWithTag:")]
		nint IndexOfItem (nint tag);

		[Export ("indexOfItemWithRepresentedObject:")]
		nint IndexOfItem (NSObject obj);

		[Export ("indexOfItemWithTarget:andAction:")]
		nint IndexOfItem (NSObject target, Selector actionSelector);

		[Export ("itemAtIndex:")]
		NSMenuItem ItemAtIndex (nint index);

		[Export ("itemWithTitle:")]
		NSMenuItem ItemWithTitle (string title);

		[Export ("lastItem")]
		NSMenuItem LastItem { get; }

		[Export ("selectItem:")]
		void SelectItem ([NullAllowed] NSMenuItem item);

		[Export ("selectItemAtIndex:")]
		void SelectItem (nint index);

		[Export ("selectItemWithTitle:")]
		void SelectItem (string title);

		[Export ("selectItemWithTag:")]
		bool SelectItemWithTag (nint tag);

		[Export ("setTitle:")]
		void SetTitle (string aString);

		[Export ("selectedItem")]
		NSMenuItem SelectedItem { get; }

		[Export ("indexOfSelectedItem")]
		nint IndexOfSelectedItem { get; }

		[Export ("synchronizeTitleAndSelectedItem")]
		void SynchronizeTitleAndSelectedItem ();

		[Export ("itemTitleAtIndex:")]
		string ItemTitle (nint index);

		[Export ("itemTitles")]
		string [] ItemTitles ();

		[Export ("titleOfSelectedItem")]
		string TitleOfSelectedItem { get; }

		//Detected properties
		[Export ("menu")]
		NSMenu Menu { get; set; }

		[Export ("pullsDown")]
		bool PullsDown { get; set; }

		[Export ("autoenablesItems")]
		bool AutoEnablesItems { get; set; }

		[Export ("preferredEdge")]
		NSRectEdge PreferredEdge { get; set; }

	}


	[BaseType (typeof (NSMenuItemCell))]
	public partial interface NSPopUpButtonCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("initTextCell:pullsDown:")]
		IntPtr Constructor (string stringValue, bool pullDown);

		[Export ("addItemWithTitle:")]
		void AddItem (string title);

		[Export ("addItemsWithTitles:")]
		void AddItems (string [] itemTitles);

		[Export ("insertItemWithTitle:atIndex:")]
		void InsertItem (string title, nint index);

		[Export ("removeItemWithTitle:")]
		void RemoveItem (string title);

		[Export ("removeItemAtIndex:")]
		void RemoveItemAt (nint index);

		[Export ("removeAllItems")]
		void RemoveAllItems ();

		[Export ("itemArray")]
		NSMenuItem [] Items { get; }

		[Export ("numberOfItems")]
		nint Count { get; }

		[Export ("indexOfItem:")]
		nint IndexOf (NSMenuItem item);

		[Export ("indexOfItemWithTitle:")]
		nint IndexOfItemWithTitle (string title);

		[Export ("indexOfItemWithTag:")]
		nint IndexOfItemWithTag (nint tag);

		[Export ("indexOfItemWithRepresentedObject:")]
		nint IndexOfItemWithRepresentedObject (NSObject obj);

		[Export ("indexOfItemWithTarget:andAction:")]
		nint IndexOfItemWithTargetandAction (NSObject target, Selector actionSelector);

		[Export ("itemAtIndex:")]
		NSMenuItem ItemAt (nint index);

		[Export ("itemWithTitle:")]
		NSMenuItem ItemWithTitle (string title);

		[Export ("lastItem")]
		NSMenuItem LastItem { get; }

		[Export ("selectItem:")]
		void SelectItem (NSMenuItem item);

		[Export ("selectItemAtIndex:")]
		void SelectItemAt (nint index);

		[Export ("selectItemWithTitle:")]
		void SelectItemWithTitle (string title);

		[Export ("selectItemWithTag:")]
		bool SelectItemWithTag (nint tag);

		[Export ("setTitle:")]
		void SetTitle (string aString);

		[Export ("selectedItem")]
		NSMenuItem SelectedItem { get; }

		[Export ("indexOfSelectedItem")]
		nint SelectedItemIndex { get; }

		[Export ("synchronizeTitleAndSelectedItem")]
		void SynchronizeTitleAndSelectedItem ();

		[Export ("itemTitleAtIndex:")]
		string GetItemTitle (int index);

		[Export ("itemTitles")]
		string [] ItemTitles { get; }

		[Export ("titleOfSelectedItem")]
		string TitleOfSelectedItem { get; }

		[Export ("attachPopUpWithFrame:inView:")]
		void AttachPopUp (CGRect cellFrame, NSView inView);

		[Export ("dismissPopUp")]
		void DismissPopUp ();

		[Export ("performClickWithFrame:inView:")]
		void PerformClick (CGRect withFrame, NSView controlView);

		//Detected properties
		[Export ("menu")]
		NSMenu Menu { get; set; }

		[Export ("pullsDown")]
		bool PullsDown { get; set; }

		[Export ("autoenablesItems")]
		bool AutoenablesItems { get; set; }

		[Export ("preferredEdge")]
		NSRectEdge PreferredEdge { get; set; }

		[Export ("usesItemFromMenu")]
		bool UsesItemFromMenu { get; set; }

		[Export ("altersStateOfSelectedItem")]
		bool AltersStateOfSelectedItem { get; set; }

		[Export ("arrowPosition")]
		NSPopUpArrowPosition ArrowPosition { get; set; }

		[Export ("objectValue")]
		NSObject ObjectValue { get; set; }

	}

	[BaseType (typeof (NSObject))]
	public interface NSPrinter {
		[Static]
		[Export ("printerNames")]
		string [] PrinterNames{ get; }

		[Static]
		[Export ("printerTypes")]
		string [] PrinterTypes { get; }

		[Static]
		[Export ("printerWithName:")]
		NSPrinter PrinterWithName (string name);

		[Static]
		[Export ("printerWithType:")]
		NSPrinter PrinterWithType (string type);

		[Export ("name")]
		string Name { get; }

		[Export ("type")]
		string Type { get; }

		[Export ("languageLevel")]
		nint LanguageLevel { get; }

		[Export ("pageSizeForPaper:")]
		CGSize PageSizeForPaper (string paperName); 

		[Export ("statusForTable:")]
		NSPrinterTableStatus StatusForTable (string tableName);

		[Export ("isKey:inTable:")]
		bool IsKeyInTable (string key, string table);

		[Export ("booleanForKey:inTable:")]
		bool BooleanForKey (string key, string table);

		[Export ("floatForKey:inTable:")]
		float FloatForKey (string key, string table);

		[Export ("intForKey:inTable:")]
		int IntForKey (string key, string table);

		[Export ("rectForKey:inTable:")]
		CGRect RectForKey (string key, string table);

		[Export ("sizeForKey:inTable:")]
		CGSize SizeForKey (string key, string table);

		[Export ("stringForKey:inTable:")]
		string StringForKey (string key, string table);

		[Export ("stringListForKey:inTable:")]
		string [] StringListForKey (string key, string table);

		[Export ("deviceDescription")]
		NSDictionary DeviceDescription { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSPrintInfo {
		[Export ("initWithDictionary:")]
		IntPtr Constructor (NSDictionary attributes);

		[Export ("dictionary")]
		NSMutableDictionary Dictionary { get; }

		[Export ("setUpPrintOperationDefaultValues")]
		void SetUpPrintOperationDefaultValues ();

		[Export ("imageablePageBounds")]
		CGRect ImageablePageBounds { get; }

		[Export ("localizedPaperName")]
		string LocalizedPaperName { get; }

		[Static]
		[Export ("defaultPrinter")]
		NSPrinter DefaultPrinter { get; }

		[Export ("printSettings")]
		NSMutableDictionary PrintSettings { get; }

		[Export ("PMPrintSession")]
		IntPtr GetPMPrintSession ();

		[Export ("PMPageFormat")]
		IntPtr GetPMPageFormat ();

		[Export ("PMPrintSettings")]
		IntPtr GetPMPrintSettings ();

		[Export ("updateFromPMPageFormat")]
		void UpdateFromPMPageFormat ();

		[Export ("updateFromPMPrintSettings")]
		void UpdateFromPMPrintSettings ();

		//Detected properties
		[Static]
		[Export ("sharedPrintInfo")]
		NSPrintInfo SharedPrintInfo { get; set; }

		[Export ("paperName")]
		string PaperName { get; set; }

		[Export ("paperSize")]
		CGSize PaperSize { get; set; }

		[Export ("orientation")]
		NSPrintingOrientation Orientation { get; set; }

		[Export ("scalingFactor")]
		nfloat ScalingFactor { get; set; }

		[Export ("leftMargin")]
		nfloat LeftMargin { get; set; }

		[Export ("rightMargin")]
		nfloat RightMargin { get; set; }

		[Export ("topMargin")]
		nfloat TopMargin { get; set; }

		[Export ("bottomMargin")]
		nfloat BottomMargin { get; set; }

		[Export ("horizontallyCentered")]
		bool HorizontallyCentered { [Bind ("isHorizontallyCentered")]get; set; }

		[Export ("verticallyCentered")]
		bool VerticallyCentered { [Bind ("isVerticallyCentered")]get; set; }

		[Export ("horizontalPagination")]
		NSPrintingPaginationMode HorizontalPagination { get; set; }

		[Export ("verticalPagination")]
		NSPrintingPaginationMode VerticalPagination { get; set; }

		[Export ("jobDisposition")]
		string JobDisposition { get; set; }

		[Export ("printer")]
		NSPrinter Printer { get; set; }

		[Export ("selectionOnly")]
		bool SelectionOnly { [Bind ("isSelectionOnly")]get; set; }

	}


	[BaseType (typeof (NSObject))]
	public partial interface NSPrintOperation {
		[Static]
		[Export ("printOperationWithView:printInfo:")]
		NSPrintOperation FromView (NSView view, NSPrintInfo printInfo);

		[Static]
		[Export ("PDFOperationWithView:insideRect:toData:printInfo:")]
		NSPrintOperation PdfFromView (NSView view, CGRect rect, NSMutableData data, NSPrintInfo printInfo);

		[Static]
		[Export ("PDFOperationWithView:insideRect:toPath:printInfo:")]
		NSPrintOperation PdfFromView (NSView view, CGRect rect, string path, NSPrintInfo printInfo);

		[Static]
		[Export ("EPSOperationWithView:insideRect:toData:printInfo:")]
		NSPrintOperation EpsFromView (NSView view, CGRect rect, NSMutableData data, NSPrintInfo printInfo);

		[Static]
		[Export ("EPSOperationWithView:insideRect:toPath:printInfo:")]
		NSPrintOperation EpsFromView (NSView view, CGRect rect, string path, NSPrintInfo printInfo);

		[Static]
		[Export ("printOperationWithView:")]
		NSPrintOperation FromView (NSView view);

		[Static]
		[Export ("PDFOperationWithView:insideRect:toData:")]
		NSPrintOperation PdfFromView (NSView view, CGRect rect, NSMutableData data);

		[Static]
		[Export ("EPSOperationWithView:insideRect:toData:")]
		NSPrintOperation EpsFromView (NSView view, CGRect rect, NSMutableData data);

		[Export ("isCopyingOperation")]
		bool IsCopyingOperation { get; }

		[Export ("runOperationModalForWindow:delegate:didRunSelector:contextInfo:")]
		void RunOperationModal (NSWindow docWindow, NSObject del, Selector didRunSelector, IntPtr contextInfo);

		[Export ("runOperation")]
		bool RunOperation ();

		[Export ("view")]
		NSView View { get; }

		[Export ("context")]
		NSGraphicsContext Context { get; }

		[Export ("pageRange")]
		NSRange PageRange { get; }

		[Export ("currentPage")]
		nint CurrentPage { get; }

		[Export ("createContext")]
		NSGraphicsContext CreateContext ();

		[Export ("destroyContext")]
		void DestroyContext ();

		[Export ("deliverResult")]
		bool DeliverResult ();

		[Export ("cleanUpOperation")]
		void CleanUpOperation ();

		//Detected properties
		[Static]
		[Export ("currentOperation")]
		NSPrintOperation CurrentOperation { get; set; }

		[Export ("jobTitle")]
		string JobTitle { get; set; }

		[Export ("showsPrintPanel")]
		bool ShowsPrintPanel { get; set; }

		[Export ("showsProgressPanel")]
		bool ShowsProgressPanel { get; set; }

		[Export ("printPanel")]
		NSPrintPanel PrintPanel { get; set; }

		[Export ("canSpawnSeparateThread")]
		bool CanSpawnSeparateThread { get; set; }

		[Export ("pageOrder")]
		NSPrintingPageOrder PageOrder { get; set; }

		[Export ("printInfo")]
		NSPrintInfo PrintInfo { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSPrintPanelAccessorizing {
		[Abstract]
		[Export ("localizedSummaryItems")]
		NSDictionary [] LocalizedSummaryItems ();

		[Abstract]
		[Export ("keyPathsForValuesAffectingPreview")]
		NSSet KeyPathsForValuesAffectingPreview ();
	}

	[BaseType (typeof (NSObject))]
	[Dispose ("__mt_accessory_var = null;")] 
	public interface NSPrintPanel {
		[Static]
		[Export ("printPanel")]
		NSPrintPanel PrintPanel { get; }

		[Export ("addAccessoryController:")]
		[PostSnippet ("__mt_accessory_var = AccessoryControllers();")]
		void AddAccessoryController (NSViewController accessoryController);

		[Export ("removeAccessoryController:")]
		[PostSnippet ("__mt_accessory_var = AccessoryControllers();")]
		void RemoveAccessoryController (NSViewController accessoryController);

		[Export ("accessoryControllers")]
		NSViewController [] AccessoryControllers ();

		[Export ("beginSheetWithPrintInfo:modalForWindow:delegate:didEndSelector:contextInfo:")]
		void BeginSheet (NSPrintInfo printInfo, NSWindow docWindow, [NullAllowed] NSObject del, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);

		[Export ("runModalWithPrintInfo:")]
		nint RunModalWithPrintInfo (NSPrintInfo printInfo);

		[Export ("runModal")]
		nint RunModal ();

		[Export ("printInfo")]
		NSPrintInfo PrintInfo { get; }

		//Detected properties
		[Export ("options")]
		NSPrintPanelOptions Options { get; set; }

		[Export ("defaultButtonTitle")]
		string DefaultButtonTitle { get; set; }

		[Export ("helpAnchor")]
		string HelpAnchor { get; set; }

		[Export ("jobStyleHint")]
		string JobStyleHint { get; set; }
	}

	[BaseType (typeof (NSView))]
	public interface NSProgressIndicator {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("incrementBy:")]
		void IncrementBy (double delta);

		[Export ("startAnimation:")]
		void StartAnimation ([NullAllowed] NSObject sender);

		[Export ("stopAnimation:")]
		void StopAnimation ([NullAllowed] NSObject sender);

		[Export ("style")]
		NSProgressIndicatorStyle Style { get; set; }

		[Export ("sizeToFit")]
		void SizeToFit ();

		[Export ("displayedWhenStopped")]
		bool IsDisplayedWhenStopped { [Bind ("isDisplayedWhenStopped")] get; set; }

		//Detected properties
		[Export ("indeterminate")]
		bool Indeterminate { [Bind ("isIndeterminate")]get; set; }

		[Export ("bezeled")]
		bool Bezeled { [Bind ("isBezeled")]get; set; }

		[Export ("controlTint")]
		NSControlTint ControlTint { get; set; }

		[Export ("controlSize")]
		NSControlSize ControlSize { get; set; }

		[Export ("doubleValue")]
		double DoubleValue { get; set; }

		[Export ("minValue")]
		double MinValue { get; set; }

		[Export ("maxValue")]
		double MaxValue { get; set; }

		[Export ("usesThreadedAnimation")]
		bool UsesThreadedAnimation { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface NSResponder {
		[Export ("tryToPerform:with:")]
		bool TryToPerformwith (Selector anAction, NSObject anObject);

		[Export ("performKeyEquivalent:")]
		bool PerformKeyEquivalent (NSEvent theEvent);

		[Export ("validRequestorForSendType:returnType:")]
		NSObject ValidRequestorForSendTypereturnType (string sendType, string returnType);

		[Export ("mouseDown:")]
		void MouseDown (NSEvent theEvent);

		[Export ("rightMouseDown:")]
		void RightMouseDown (NSEvent theEvent);

		[Export ("otherMouseDown:")]
		void OtherMouseDown (NSEvent theEvent);

		[Export ("mouseUp:")]
		void MouseUp (NSEvent theEvent);

		[Export ("rightMouseUp:")]
		void RightMouseUp (NSEvent theEvent);

		[Export ("otherMouseUp:")]
		void OtherMouseUp (NSEvent theEvent);

		[Export ("mouseMoved:")]
		void MouseMoved (NSEvent theEvent);

		[Export ("mouseDragged:")]
		void MouseDragged (NSEvent theEvent);

		[Export ("scrollWheel:")]
		void ScrollWheel (NSEvent theEvent);

		[Export ("rightMouseDragged:")]
		void RightMouseDragged (NSEvent theEvent);

		[Export ("otherMouseDragged:")]
		void OtherMouseDragged (NSEvent theEvent);

		[Export ("mouseEntered:")]
		void MouseEntered (NSEvent theEvent);

		[Export ("mouseExited:")]
		void MouseExited (NSEvent theEvent);

		[Export ("keyDown:")]
		void KeyDown (NSEvent theEvent);

		[Export ("keyUp:")]
		void KeyUp (NSEvent theEvent);

		[Export ("flagsChanged:")]
		void FlagsChanged (NSEvent theEvent);

		[Export ("tabletPoint:")]
		void TabletPoint (NSEvent theEvent);

		[Export ("tabletProximity:")]
		void TabletProximity (NSEvent theEvent);

		[Export ("cursorUpdate:")]
		void CursorUpdate (NSEvent theEvent);

		[Export ("magnifyWithEvent:")]
		void MagnifyWithEvent (NSEvent theEvent);

		[Export ("rotateWithEvent:")]
		void RotateWithEvent (NSEvent theEvent);

		[Export ("swipeWithEvent:")]
		void SwipeWithEvent (NSEvent theEvent);

		[Export ("beginGestureWithEvent:")]
		void BeginGestureWithEvent (NSEvent theEvent);

		[Export ("endGestureWithEvent:")]
		void EndGestureWithEvent (NSEvent theEvent);

		[Export ("touchesBeganWithEvent:")]
		void TouchesBeganWithEvent (NSEvent theEvent);

		[Export ("touchesMovedWithEvent:")]
		void TouchesMovedWithEvent (NSEvent theEvent);

		[Export ("touchesEndedWithEvent:")]
		void TouchesEndedWithEvent (NSEvent theEvent);

		[Export ("touchesCancelledWithEvent:")]
		void TouchesCancelledWithEvent (NSEvent theEvent);

		[Export ("noResponderFor:")]
		void NoResponderFor (Selector eventSelector);

		[Export ("acceptsFirstResponder")]
		bool AcceptsFirstResponder ();

		[Export ("becomeFirstResponder")]
		bool BecomeFirstResponder ();

		[Export ("resignFirstResponder")]
		bool ResignFirstResponder ();

		[Export ("interpretKeyEvents:")]
		void InterpretKeyEvents (NSEvent [] eventArray);

		[Export ("flushBufferedKeyEvents")]
		void FlushBufferedKeyEvents ();

		[Export ("showContextHelp:")]
		void ShowContextHelp (NSObject sender);

		[Export ("helpRequested:")]
		void HelpRequested (NSEvent theEventPtr);

		[Export ("shouldBeTreatedAsInkEvent:")]
		bool ShouldBeTreatedAsInkEvent (NSEvent theEvent);

		//Detected properties
		[Export ("nextResponder")][NullAllowed]
		NSResponder NextResponder { get; set; }

		[Export ("menu")][NullAllowed]
		NSMenu Menu { get; set; }

		[Lion, Export ("encodeRestorableStateWithCoder:")]
		void EncodeRestorableState (NSCoder coder);

		[Lion, Export ("restoreStateWithCoder:")]
		void RestoreState (NSCoder coder);

		[Lion, Export ("invalidateRestorableState")]
		void InvalidateRestorableState ();

		[Static]
		[Lion, Export ("restorableStateKeyPaths")]
		string [] RestorableStateKeyPaths ();

		[Lion]
		[Export ("wantsForwardedScrollEventsForAxis:")]
		bool WantsForwardedScrollEventsForAxis (NSEventGestureAxis axis);
	}


	[BaseType (typeof (NSObject))]
	public interface NSRulerMarker {
		[Export ("initWithRulerView:markerLocation:image:imageOrigin:")]
		IntPtr Constructor (NSRulerView ruler, nfloat location, NSImage image, CGPoint imageOrigin);

		[Export ("ruler")]
		NSRulerView Ruler { get; }

		[Export ("isDragging")]
		bool IsDragging { get; }

		[Export ("imageRectInRuler")]
		CGRect ImageRectInRuler { get; }

		[Export ("thicknessRequiredInRuler")]
		nfloat ThicknessRequiredInRuler { get; }

		[Export ("drawRect:")]
		void DrawRect (CGRect rect);

		[Export ("trackMouse:adding:")]
		bool TrackMouse (NSEvent mouseDownEvent, bool isAdding);

		//Detected properties
		[Export ("markerLocation")]
		nfloat MarkerLocation { get; set; }

		[Export ("image")]
		NSImage Image { get; set; }

		[Export ("imageOrigin")]
		CGPoint ImageOrigin { get; set; }

		[Export ("movable")]
		bool Movable { [Bind ("isMovable")]get; set; }

		[Export ("removable")]
		bool Removable { [Bind ("isRemovable")]get; set; }

		[Export ("representedObject")]
		NSObject RepresentedObject { get; set; }
	}

	[BaseType (typeof (NSView))]
	public partial interface NSRulerView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Static]
		[Export ("registerUnitWithName:abbreviation:unitToPointsConversionFactor:stepUpCycle:stepDownCycle:")]
		void RegisterUnit (string unitName, string abbreviation, nfloat conversionFactor, NSNumber [] stepUpCycle, NSNumber [] stepDownCycle);

		[Export ("initWithScrollView:orientation:")]
		IntPtr Constructor (NSScrollView scrollView, NSRulerOrientation orientation);

		[Export ("baselineLocation")]
		nfloat BaselineLocation { get; }

		[Export ("requiredThickness")]
		nfloat RequiredThickness { get; }

		[Export ("addMarker:")]
		[PostGet ("Markers")]
		void AddMarker (NSRulerMarker marker);

		[Export ("removeMarker:")]
		[PostGet ("Markers")]
		void RemoveMarker (NSRulerMarker marker);

		[Export ("trackMarker:withMouseEvent:")]
		bool TrackMarker (NSRulerMarker marker, NSEvent theEvent);

		[Export ("moveRulerlineFromLocation:toLocation:")]
		void MoveRulerline (nfloat oldLocation, nfloat newLocation);

		[Export ("invalidateHashMarks")]
		void InvalidateHashMarks ();

		[Export ("drawHashMarksAndLabelsInRect:")]
		void DrawHashMarksAndLabels (CGRect rect);

		[Export ("drawMarkersInRect:")]
		void DrawMarkers (CGRect rect);

		[Export ("isFlipped")]
		bool IsFlipped { get; }

		//Detected properties
		[Export ("scrollView")]
		NSScrollView ScrollView { get; set; }

		[Export ("orientation")]
		NSRulerOrientation Orientation { get; set; }

		[Export ("ruleThickness")]
		nfloat RuleThickness { get; set; }

		[Export ("reservedThicknessForMarkers")]
		nfloat ReservedThicknessForMarkers { get; set; }

		[Export ("reservedThicknessForAccessoryView")]
		nfloat ReservedThicknessForAccessoryView { get; set; }

		[Export ("measurementUnits")]
		string MeasurementUnits { get; set; }

		[Export ("originOffset")]
		nfloat OriginOffset { get; set; }

		[Export ("clientView")]
		NSView ClientView { get; set; }

		[Export ("markers"), NullAllowed]
		NSRulerMarker [] Markers { get; set; }

		[Export ("accessoryView"), NullAllowed]
		NSView AccessoryView { get; set; }
	}

	public delegate void NSSavePanelComplete (int result);
	
	[BaseType (typeof (NSPanel), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSOpenSavePanelDelegate)})]
	public interface NSSavePanel {
		[Static]
		[Export ("savePanel")]
		NSSavePanel SavePanel { get; }

		[Export ("URL")]
		NSUrl Url { get; }

		[Export ("isExpanded")]
		bool IsExpanded { get; }

		[Export ("validateVisibleColumns")]
		void ValidateVisibleColumns ();

		[Export ("ok:")]
		void Ok (NSObject sender);

		[Export ("cancel:")]
		void Cancel (NSObject sender);

		[Export ("beginSheetModalForWindow:completionHandler:")]
		void BeginSheet (NSWindow window, NSSavePanelComplete onComplete);

		[Export ("beginWithCompletionHandler:")]
		void Begin (NSSavePanelComplete onComplete);

		[Export ("runModal")]
		nint RunModal ();

		//Detected properties
		[Export ("directoryURL")]
		NSUrl DirectoryUrl { get; set; }

		[Export ("allowedFileTypes"), NullAllowed]
		string [] AllowedFileTypes { get; set; }

		[Export ("allowsOtherFileTypes")]
		bool AllowsOtherFileTypes { get; set; }

		[Export ("accessoryView"), NullAllowed]
		NSView AccessoryView { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSOpenSavePanelDelegate Delegate { get; set; }

		[Export ("canCreateDirectories")]
		bool CanCreateDirectories { get; set; }

		[Export ("canSelectHiddenExtension")]
		bool CanSelectHiddenExtension { get; set; }

		[Export ("extensionHidden")]
		bool ExtensionHidden { [Bind ("isExtensionHidden")]get; set; }

		[Export ("treatsFilePackagesAsDirectories")]
		bool TreatsFilePackagesAsDirectories { get; set; }

		[Export ("prompt")]
		string Prompt { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("nameFieldLabel")]
		string NameFieldLabel { get; set; }

		[Export ("nameFieldStringValue")]
		string NameFieldStringValue { get; set; }

		[Export ("message")]
		string Message { get; set; }

		[Export ("showsHiddenFiles")]
		bool ShowsHiddenFiles { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 6, message: "use Url instead")]
		[Export ("filename")]
		string Filename { get; }

		[Deprecated (PlatformName.MacOSX, 10, 6, message: "use DirectoryUrl instead")]
		[Export ("directory")]
		string Directory { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 6, message: "use AllowedFileTypes instead")]
		[Export ("requiredFileType")]
		string RequiredFileType { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 6, message: "use Begin with the callback")]
		[Export ("beginSheetForDirectory:file:modalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		void Begin (string directory, string filename, NSWindow docWindow, NSObject modalDelegate, Selector selector, IntPtr context);

		[Deprecated (PlatformName.MacOSX, 10, 6, message: "use RunModal without parameters instead")]
		[Export ("runModalForDirectory:file:")]
		nint RunModal ([NullAllowed] string directory, [NullAllowed]  string filename);
	}

	[BaseType (typeof (NSSavePanel))]
	[DisableDefaultCtor] // should not be created by (only returned to) user code
	public interface NSRemoteSavePanel {}

	[BaseType (typeof (NSObject))]
	public partial interface NSScreen {
		[Static]
		[Export ("screens")]
		NSScreen [] Screens { get; }

		[Static]
		[Export ("mainScreen")]
		NSScreen MainScreen { get; }

		[Static]
		[Export ("deepestScreen")]
		NSScreen DeepestScreen { get; }

		[Export ("depth")]
		NSWindowDepth Depth { get; }

		[Export ("frame")]
		CGRect Frame { get; }

		[Export ("visibleFrame")]
		CGRect VisibleFrame { get; }

		[Export ("deviceDescription")]
		NSDictionary DeviceDescription { get; }

		[Export ("colorSpace")]
		NSColorSpace ColorSpace { get; }

		[Export ("supportedWindowDepths"), Internal]
		IntPtr GetSupportedWindowDepths ();

		[Export ("userSpaceScaleFactor"), Obsolete ("On Lion")]
		nfloat UserSpaceScaleFactor { get; }

		[Lion, Export ("convertRectToBacking:")]
		CGRect ConvertRectToBacking (CGRect aRect);

		[Lion, Export ("convertRectFromBacking:")]
		CGRect ConvertRectfromBacking (CGRect aRect);

		[Lion, Export ("backingAlignedRect:options:")]
		CGRect GetBackingAlignedRect (CGRect globalScreenCoordRect, NSAlignmentOptions options);

		[Lion, Export ("backingScaleFactor")]
		nfloat BackingScaleFactor { get; }
	}

	[BaseType (typeof (NSControl))]
	public interface NSScroller {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Static]
		[Export ("scrollerWidth")]
		nfloat ScrollerWidth { get; }

		[Static]
		[Export ("scrollerWidthForControlSize:")]
		nfloat ScrollerWidthForControlSize (NSControlSize controlSize);

		[Export ("drawParts")]
		[Obsolete]
		void DrawParts ();

		[Export ("rectForPart:")]
		CGRect RectForPart (NSScrollerPart partCode);

		[Export ("checkSpaceForParts")]
		void CheckSpaceForParts ();

		[Export ("usableParts")]
		NSUsableScrollerParts UsableParts { get; }

		[Export ("drawArrow:highlight:")]
		void DrawArrow (NSScrollerArrow whichArrow, bool highlight);

		[Export ("drawKnob")]
		void DrawKnob ();

		[Export ("drawKnobSlotInRect:highlight:")]
		void DrawKnobSlot (CGRect slotRect, bool highlight);

		[Export ("highlight:")]
		void Highlight (bool flag);

		[Export ("testPart:")]
		NSScrollerPart TestPart (CGPoint thePoint);

		[Export ("trackKnob:")]
		void TrackKnob (NSEvent theEvent);

		[Export ("trackScrollButtons:")]
		void TrackScrollButtons (NSEvent theEvent);

		[Export ("hitPart")]
		NSScrollerPart HitPart { get; }

		//Detected properties
		[Export ("arrowsPosition")]
		NSScrollArrowPosition ArrowsPosition { get; set; }

		[Export ("controlTint")]
		NSControlTint ControlTint { get; set; }

		[Export ("controlSize")]
		NSControlSize ControlSize { get; set; }

		[Export ("knobProportion")]
		nfloat KnobProportion { get; set; }
		
		[Static]
		[Lion, Export ("isCompatibleWithOverlayScrollers")]
		bool CompatibleWithOverlayScrollers { get; }
		
		[Lion, Export ("knobStyle")]
		NSScrollerKnobStyle KnobStyle { get; set; }
		
		[Static]
		[Lion, Export ("preferredScrollerStyle")]
		NSScrollerStyle PreferredScrollerStyle { get; }
		
		[Export ("scrollerStyle")]
		NSScrollerStyle ScrollerStyle { get; set; }
		
		[Static]
		[Lion, Export ("scrollerWidthForControlSize:scrollerStyle:")]
		nfloat GetScrollerWidth (NSControlSize forControlSize, NSScrollerStyle scrollerStyle);
		
		[Notification, Field ("NSPreferredScrollerStyleDidChangeNotification")]
		NSString PreferredStyleChangedNotification { get; }

	}

	[BaseType (typeof (NSView))]
	public partial interface NSScrollView : NSTextFinderBarContainer {
		[Static]
		[Export ("frameSizeForContentSize:hasHorizontalScroller:hasVerticalScroller:borderType:")]
		CGSize FrameSizeForContentSize (CGSize cSize, bool hFlag, bool vFlag, NSBorderType aType);

		[Static]
		[Export ("contentSizeForFrameSize:hasHorizontalScroller:hasVerticalScroller:borderType:")]
		CGSize ContentSizeForFrame (CGSize fSize, bool hFlag, bool vFlag, NSBorderType aType);

		[Export ("documentVisibleRect")]
		CGRect DocumentVisibleRect { get; }

		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("contentSize")]
		CGSize ContentSize { get; }

		[Export ("tile")]
		void Tile ();

		[Export ("reflectScrolledClipView:")]
		void ReflectScrolledClipView (NSClipView cView);

		[Export ("scrollWheel:")]
		void ScrollWheel (NSEvent theEvent);

		//Detected properties
		[Export ("documentView")]
		NSObject DocumentView { get; set; }

		[Export ("contentView")]
		NSClipView ContentView { get; set; }

		[Export ("documentCursor")]
		NSCursor DocumentCursor { get; set; }

		[Export ("borderType")]
		NSBorderType BorderType { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("hasVerticalScroller")]
		bool HasVerticalScroller { get; set; }

		[Export ("hasHorizontalScroller")]
		bool HasHorizontalScroller { get; set; }
		
		[Export ("verticalScroller")]
		NSScroller VerticalScroller { get; set; }

		[Export ("horizontalScroller")]
		NSScroller HorizontalScroller { get; set; }

		[Export ("autohidesScrollers")]
		bool AutohidesScrollers { get; set; }

		[Export ("horizontalLineScroll")]
		nfloat HorizontalLineScroll { get; set; }

		[Export ("verticalLineScroll")]
		nfloat VerticalLineScroll { get; set; }

		[Export ("lineScroll")]
		nfloat LineScroll { get; set; }

		[Export ("horizontalPageScroll")]
		nfloat HorizontalPageScroll { get; set; }

		[Export ("verticalPageScroll")]
		nfloat VerticalPageScroll { get; set; }

		[Export ("pageScroll")]
		nfloat PageScroll { get; set; }

		[Export ("scrollsDynamically")]
		bool ScrollsDynamically { get; set; }
		
		[Export ("hasVerticalRuler")]
		bool HasVerticalRuler { get; set; }

		[Export ("hasHorizontalRuler")]
		bool HasHorizontalRuler { get; set; }
		
		[Export ("rulersVisible")]
		bool RulersVisible { get; set; }
		
		[Export ("horizontalRulerView")]
		NSRulerView HorizontalRulerView { get; set; }
		
		[Export ("verticalRulerView")]
		NSRulerView VerticalRulerView { get; set; }

		[Static]
		[Lion, Export ("contentSizeForFrameSize:horizontalScrollerClass:verticalScrollerClass:borderType:controlSize:scrollerStyle:")]
		CGSize GetContentSizeForFrame (CGSize forFrameSize, Class horizontalScrollerClass, Class verticalScrollerClass, NSBorderType borderType, NSControlSize controlSize, NSScrollerStyle scrollerStyle);
        
        	[Lion, Export ("findBarPosition")]
        	NSScrollViewFindBarPosition FindBarPosition { get; set; }

        	[Lion, Export ("flashScrollers")]
        	void FlashScrollers ();
        
		[Static]
		[Lion, Export ("frameSizeForContentSize:horizontalScrollerClass:verticalScrollerClass:borderType:controlSize:scrollerStyle:")]
		CGSize GetFrameSizeForContent (CGSize contentSize, Class horizontalScrollerClass, Class verticalScrollerClass, NSBorderType borderType, NSControlSize controlSize, NSScrollerStyle scrollerStyle);
		
		[Lion, Export ("horizontalScrollElasticity")]
		NSScrollElasticity HorizontalScrollElasticity { get; set; }
        
        	[Lion, Export ("scrollerKnobStyle")]
        	NSScrollerKnobStyle ScrollerKnobStyle { get; set; }
        
        	[Lion, Export ("scrollerStyle")]
        	NSScrollerStyle ScrollerStyle { get; set; }
        
		[Lion, Export ("usesPredominantAxisScrolling")]
        	bool UsesPredominantAxisScrolling { get; set; }

		[Lion, Export ("verticalScrollElasticity")]
		NSScrollElasticity VerticalScrollElasticity { get; set; }
		
		[Export ("allowsMagnification")]
		bool AllowsMagnification { get; set; }

		[Export ("magnification")]
		nfloat Magnification { get; set; }

		[Export ("maxMagnification")]
		nfloat MaxMagnification { get; set; }

		[Export ("minMagnification")]
		nfloat MinMagnification { get; set; }

		[Export ("magnifyToFitRect:")]
		void MagnifyToFitRect (CGRect rect);

		[Export ("setMagnification:centeredAtPoint:")]
		void SetMagnification (nfloat magnification, CGPoint centeredAtPoint);

		[Notification, Field ("NSScrollViewWillStartLiveMagnifyNotification")]
		NSString WillStartLiveMagnifyNotification { get; }

		[Notification, Field ("NSScrollViewDidEndLiveMagnifyNotification")]
		NSString DidEndLiveMagnifyNotification { get; }
		
		[Notification, Field ("NSScrollViewWillStartLiveScrollNotification")]
		NSString WillStartLiveScrollNotification { get; }

		[Mac (10,9), Notification, Field ("NSScrollViewDidLiveScrollNotification")]
		NSString DidLiveScrollNotification { get; }

		[Mac (10,9), Notification, Field ("NSScrollViewDidEndLiveScrollNotification")]
		NSString DidEndLiveScrollNotification { get; }

		[Mac (10,10)]
		[Export ("automaticallyAdjustsContentInsets")]
		bool AutomaticallyAdjustsContentInsets { get; set; }

		// @property NSEdgeInsets contentInsets __attribute__((availability(macosx, introduced=10_10)));
		[Mac (10, 10)]
		[Export ("contentInsets", ArgumentSemantic.Assign)]
		NSEdgeInsets ContentInsets { get; set; }

		// @property NSEdgeInsets scrollerInsets __attribute__((availability(macosx, introduced=10_10)));
		[Mac (10, 10)]
		[Export ("scrollerInsets", ArgumentSemantic.Assign)]
		NSEdgeInsets ScrollerInsets { get; set; }

		[Mac (10,9)]
		[Export ("addFloatingSubview:forAxis:")]
		void AddFloatingSubview (NSView view, NSEventGestureAxis axis);
	}

	[BaseType (typeof (NSTextField))]
	public interface NSSearchField {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("recentSearches")]
		string [] RecentSearches { get; set; }

		[Export ("recentsAutosaveName")]
		string RecentsAutosaveName { get; set; }

		[New, Export ("cell")]
		NSSearchFieldCell Cell { get; set; }
	}

	[BaseType (typeof (NSTextFieldCell))]
	public interface NSSearchFieldCell {
		[Export ("searchButtonCell")]
		NSButtonCell SearchButtonCell { get; set; }

		[Export ("cancelButtonCell")]
		NSButtonCell CancelButtonCell { get; set; }

		[Export ("resetSearchButtonCell")]
		void ResetSearchButtonCell ();

		[Export ("resetCancelButtonCell")]
		void ResetCancelButtonCell ();

		[Export ("searchTextRectForBounds:")]
		CGRect SearchTextRectForBounds (CGRect rect);

		[Export ("searchButtonRectForBounds:")]
		CGRect SearchButtonRectForBounds (CGRect rect);

		[Export ("cancelButtonRectForBounds:")]
		CGRect CancelButtonRectForBounds (CGRect rect);

		[Export ("searchMenuTemplate")]
		NSMenu SearchMenuTemplate { get; set; }

		[Export ("sendsWholeSearchString")]
		bool SendsWholeSearchString { get; set; }

		[Export ("maximumRecents")]
		nint MaximumRecents { get; set; }

		[Export ("recentSearches")]
		string [] RecentSearches { get; set; }

		[Export ("recentsAutosaveName")]
		string RecentsAutosaveName { get; set; }

		[Export ("sendsSearchStringImmediately")]
		bool SendsSearchStringImmediately { get; set; }
	}
	
	[BaseType (typeof (NSControl))]
	public interface NSSegmentedControl {
	[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("selectSegmentWithTag:")]
		bool SelectSegment (nint tag);

		[Export ("setWidth:forSegment:")]
		void SetWidth (nfloat width, nint segment);

		[Export ("widthForSegment:")]
		nfloat GetWidth (nint segment);

		[Export ("setImage:forSegment:")]
		void SetImage ([NullAllowed] NSImage image, nint segment);

		[Export ("imageForSegment:")]
		NSImage GetImage (nint segment);

		[Export ("setImageScaling:forSegment:")]
		void SetImageScaling (NSImageScaling scaling, nint segment);

		[Export ("imageScalingForSegment:")]
		NSImageScaling GetImageScaling (nint segment);

		[Export ("setLabel:forSegment:")]
		void SetLabel (string label, nint segment);

		[Export ("labelForSegment:")]
		string GetLabel (nint segment);

		[Export ("setMenu:forSegment:")]
		void SetMenu ([NullAllowed] NSMenu menu, nint segment);

		[Export ("menuForSegment:")]
		NSMenu GetMenu (nint segment);

		[Export ("setSelected:forSegment:")]
		void SetSelected (bool selected, nint segment);

		[Export ("isSelectedForSegment:")]
		bool IsSelectedForSegment (nint segment);

		[Export ("setEnabled:forSegment:")]
		void SetEnabled (bool enabled, nint segment);

		[Export ("isEnabledForSegment:")]
		bool IsEnabled (nint segment);

		//Detected properties
		[Export ("segmentCount")]
		nint SegmentCount { get; set; }

		[Export ("selectedSegment")]
		nint SelectedSegment { get; set; }

		[Export ("segmentStyle")]
		NSSegmentStyle SegmentStyle { get; set; }

		[Mac (10,10,3)]
		[Export ("springLoaded")]
		bool IsSpringLoaded { [Bind ("isSpringLoaded")] get; set; }

		[Mac (10,10,3)]
		[Export ("trackingMode")]
		NSSegmentSwitchTracking TrackingMode { get; set; }

		[Mac (10,10,3)]
		[Export ("doubleValueForSelectedSegment")]
		double GetValueForSelectedSegment (); // actually returns double

		[Mac (10,12)]
		[Static]
		[Internal]
		[Export ("segmentedControlWithLabels:trackingMode:target:action:")]
		NSSegmentedControl _FromLabels (string[] labels, NSSegmentSwitchTracking trackingMode, [NullAllowed] NSObject target, [NullAllowed] Selector action);

		[Mac (10,12)]
		[Static]
		[Internal]
		[Export ("segmentedControlWithImages:trackingMode:target:action:")]
		NSSegmentedControl _FromImages (NSImage[] images, NSSegmentSwitchTracking trackingMode, [NullAllowed] NSObject target, [NullAllowed] Selector action);

		[Mac (10, 12, 2)]
		[NullAllowed, Export ("selectedSegmentBezelColor", ArgumentSemantic.Copy)]
		NSColor SelectedSegmentBezelColor { get; set; }

		[Mac (10,13)]
		[Export ("setToolTip:forSegment:")]
		void SetToolTip ([NullAllowed] string toolTip, nint segment);

		[Mac (10,13)]
		[Export ("toolTipForSegment:")]
		[return: NullAllowed]
		string GetToolTip (nint forSegment);

		[Mac (10,13)]
		[Export ("setTag:forSegment:")]
		void SetTag (nint tag, nint segment);

		[Mac (10,13)]
		[Export ("tagForSegment:")]
		nint GetTag (nint segment);

		[Mac (10,13)]
		[Export ("setShowsMenuIndicator:forSegment:")]
		void SetShowsMenuIndicator (bool showsMenuIndicator, nint segment);

		[Mac (10,13)]
		[Export ("showsMenuIndicatorForSegment:")]
		bool ShowsMenuIndicator (nint segment);

		[Mac (10,13)]
		[Export ("setAlignment:forSegment:")]
		void SetAlignment (NSTextAlignment alignment, nint segment);

		[Mac (10,13)]
		[Export ("alignmentForSegment:")]
		NSTextAlignment GetAlignment (nint segment);

		[Mac (10, 13)]
		[Export ("segmentDistribution", ArgumentSemantic.Assign)]
		NSSegmentDistribution SegmentDistribution { get; set; }
	}
	
	[BaseType (typeof (NSActionCell))]
	public interface NSSegmentedCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("selectSegmentWithTag:")]
		bool SelectSegment (nint tag);

		[Export ("makeNextSegmentKey")]
		void InsertSegmentAfterSelection ();

		[Export ("makePreviousSegmentKey")]
		void InsertSegmentBeforeSelection ();

		[Export ("setWidth:forSegment:")]
		void SetWidth (nfloat width, nint forSegment);

		[Export ("widthForSegment:")]
		nfloat GetWidth (nint forSegment);

		[Export ("setImage:forSegment:")]
		void SetImage (NSImage image, nint forSegment);

		[Export ("imageForSegment:")]
		NSImage GetImageForSegment (nint forSegment);

		[Export ("setImageScaling:forSegment:")]
		void SetImageScaling (NSImageScaling scaling, nint forSegment);

		[Export ("imageScalingForSegment:")]
		NSImageScaling GetImageScaling (nint forSegment);

		[Export ("setLabel:forSegment:")]
		void SetLabel (string label, nint forSegment);

		[Export ("labelForSegment:")]
		string GetLabel (nint forSegment);

		[Export ("setSelected:forSegment:")]
		void SetSelected (bool selected, nint forSegment);

		[Export ("isSelectedForSegment:")]
		bool IsSelected (nint forSegment);

		[Export ("setEnabled:forSegment:")]
		void SetEnabled (bool enabled, nint forSegment);

		[Export ("isEnabledForSegment:")]
		bool IsEnabled (nint forSegment);

		[Export ("setMenu:forSegment:")]
		void SetMenu (NSMenu menu, nint forSegment);

		[Export ("menuForSegment:")]
		NSMenu GetMenu (nint forSegment);

		[Export ("setToolTip:forSegment:")]
		void SetToolTip (string toolTip, nint forSegment);

		[Export ("toolTipForSegment:")]
		string GetToolTip (nint forSegment);

		[Export ("setTag:forSegment:")]
		void SetTag (nint tag, nint forSegment);

		[Export ("tagForSegment:")]
		nint GetTag (nint forSegment);

		[Export ("drawSegment:inFrame:withView:")]
		void DrawSegment (nint segment, CGRect frame, NSView controlView);

		//Detected properties
		[Export ("segmentCount")]
		nint SegmentCount { get; set; }

		[Export ("selectedSegment")]
		nint SelectedSegment { get; set; }

		[Export ("trackingMode")]
		NSSegmentSwitchTracking TrackingMode { get; set; }

		[Export ("segmentStyle")]
		NSSegmentStyle SegmentStyle { get; set; }

	}

	[BaseType (typeof (NSControl))]
	public interface NSSlider {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("isVertical")]
		nint IsVertical { get; }

		[Export ("acceptsFirstMouse:")]
		bool AcceptsFirstMouse (NSEvent theEvent);

		//Detected properties
		[Export ("minValue")]
		double MinValue { get; set; }

		[Export ("maxValue")]
		double MaxValue { get; set; }

		[Export ("altIncrementValue")]
		double AltIncrementValue { get; set; }

		[Export ("titleCell")]
		NSObject TitleCell { get; set; }

		[Export ("titleColor")]
		NSColor TitleColor { get; set; }

		[Export ("titleFont")]
		NSFont TitleFont { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("knobThickness")]
		nfloat KnobThickness { get; set; }

		[Export ("image")]
		NSImage Image { get; set; }
	
		[Export ("tickMarkValueAtIndex:")]
		double TickMarkValue (nint index);

		[Export ("rectOfTickMarkAtIndex:")]
		CGRect RectOfTick (nint index);

		[Export ("indexOfTickMarkAtPoint:")]
		nint IndexOfTickMark (CGPoint point);

		[Export ("closestTickMarkValueToValue:")]
		double ClosestTickMarkValue (double value);

		//Detected properties
		[Export ("numberOfTickMarks")]
		nint TickMarksCount { get; set; }

		[Export ("tickMarkPosition")]
		NSTickMarkPosition TickMarkPosition { get; set; }

		[Export ("allowsTickMarkValuesOnly")]
		bool AllowsTickMarkValuesOnly { get; set; }

	}
	
	[BaseType (typeof (NSActionCell))]
	public interface NSSliderCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Static]
		[Export ("prefersTrackingUntilMouseUp")]
		bool PrefersTrackingUntilMouseUp ();

		[Export ("isVertical")]
		int IsVertical { get; }

		[Export ("knobRectFlipped:")]
		CGRect KnobRectFlipped (bool flipped);

		[Export ("drawKnob:")]
		void DrawKnob (CGRect knobRect);

		[Export ("drawKnob")]
		void DrawKnob ();

		[Export ("drawBarInside:flipped:")]
		void DrawBar (CGRect aRect, bool flipped);

		[Export ("trackRect")]
		CGRect TrackRect{ get; }

		//Detected properties
		[Export ("minValue")]
		double MinValue { get; set; }

		[Export ("maxValue")]
		double MaxValue { get; set; }

		[Export ("altIncrementValue")]
		double AltIncrementValue { get; set; }

		[Export ("titleColor")]
		NSColor TitleColor { get; set; }

		[Export ("titleFont")]
		NSFont TitleFont { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("titleCell")]
		NSObject TitleCell { get; set; }

		[Export ("knobThickness")]
		nfloat KnobThickness { get; set; }

		[Export ("sliderType")]
		NSSliderType SliderType { get; set; }
	
		[Export ("tickMarkValueAtIndex:")]
		double TickMarkValue (nint index);

		[Export ("rectOfTickMarkAtIndex:")]
		CGRect RectOfTickMark (nint index);

		[Export ("indexOfTickMarkAtPoint:")]
		nint IndexOfTickMark (CGPoint point);

		[Export ("closestTickMarkValueToValue:")]
		double ClosestTickMarkValue (double value);

		//Detected properties
		[Export ("numberOfTickMarks")]
		nint TickMarks { get; set; }

		[Export ("tickMarkPosition")]
		NSTickMarkPosition TickMarkPosition { get; set; }

		[Export ("allowsTickMarkValuesOnly")]
		bool AllowsTickMarkValuesOnly { get; set; }

	}
	
	[BaseType (typeof (NSObject))]
	public interface NSSpeechRecognizer {
		[Export ("startListening")]
		void StartListening ();

		[Export ("stopListening")]
		void StopListening ();

		//Detected properties
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSSpeechRecognizerDelegate Delegate { get; set; }

		[Export ("commands")]
		string [] Commands { get; set; }

		[Export ("displayedCommandsTitle")]
		string DisplayedCommandsTitle { get; set; }

		[Export ("listensInForegroundOnly")]
		bool ListensInForegroundOnly { get; set; }

		[Export ("blocksOtherRecognizers")]
		bool BlocksOtherRecognizers { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSSpeechRecognizerDelegate {
		[Export ("speechRecognizer:didRecognizeCommand:")]
		void DidRecognizeCommand (NSSpeechRecognizer sender, string command);
	}

	[BaseType (typeof (NSObject))]
	public interface NSSpeechSynthesizer {
		[Export ("initWithVoice:")]
		IntPtr Constructor (string voice);

		[Export ("startSpeakingString:")]
		bool StartSpeakingString (string theString);

		[Export ("startSpeakingString:toURL:")]
		bool StartSpeakingStringtoURL (string theString, NSUrl url);

		[Export ("isSpeaking")]
		bool IsSpeaking { get; }

		[Export ("stopSpeaking")]
		void StopSpeaking ();

		[Export ("stopSpeakingAtBoundary:")]
		void StopSpeaking (NSSpeechBoundary boundary);

		[Export ("pauseSpeakingAtBoundary:")]
		void PauseSpeaking (NSSpeechBoundary boundary);

		[Export ("continueSpeaking")]
		void ContinueSpeaking ();

		[Export ("addSpeechDictionary:")]
		void AddSpeechDictionary (NSDictionary speechDictionary);

		[Export ("phonemesFromText:")]
		string PhonemesFromText (string text);

		[Export ("objectForProperty:error:")]
		NSObject ObjectForProperty (string property, out NSError outError);

		[Export ("setObject:forProperty:error:")]
		bool SetObjectforProperty (NSObject theObject, string property, out NSError outError);

		[Static]
		[Export ("isAnyApplicationSpeaking")]
		bool IsAnyApplicationSpeaking { get; }

		[Static]
		[Export ("defaultVoice")]
		string DefaultVoice { get; }

		[Static]
		[Export ("availableVoices")]
		string [] AvailableVoices { get; }

		[Static]
		[Export ("attributesForVoice:")]
		NSDictionary AttributesForVoice (string voice);

		//Detected properties
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSSpeechSynthesizerDelegate Delegate { get; set; }

		[Export ("voice"), Protected]
		string GetVoice ();

		[Export ("setVoice:"), Protected]
		bool SetVoice (string voice);

		[Export ("rate")]
		float Rate { get; set; } // 32-bit

		[Export ("volume")]
		float Volume { get; set; } // 32-bit

		[Export ("usesFeedbackWindow")]
		bool UsesFeedbackWindow { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSSpeechSynthesizerDelegate {
		[Export ("speechSynthesizer:didFinishSpeaking:")]
		void DidFinishSpeaking (NSSpeechSynthesizer sender, bool finishedSpeaking);

		[Export ("speechSynthesizer:willSpeakWord:ofString:")]
		void WillSpeakWord (NSSpeechSynthesizer sender, NSRange wordCharacterRange, string ofString);

		[Export ("speechSynthesizer:willSpeakPhoneme:")]
		void WillSpeakPhoneme (NSSpeechSynthesizer sender, short phonemeOpcode);

		[Export ("speechSynthesizer:didEncounterErrorAtIndex:ofString:message:")]
		void DidEncounterError (NSSpeechSynthesizer sender, uint characterIndex, string theString, string message);

		[Export ("speechSynthesizer:didEncounterSyncMessage:")]
		void DidEncounterSyncMessage (NSSpeechSynthesizer sender, string message);
	}

	[BaseType (typeof (NSObject))]
	public partial interface NSSpellChecker {
		[Static]
		[Export ("sharedSpellChecker")]
		NSSpellChecker SharedSpellChecker { get; }

		[Static]
		[Export ("sharedSpellCheckerExists")]
		bool SharedSpellCheckerExists { get; }

		[Static]
		[Export ("uniqueSpellDocumentTag")]
		nint UniqueSpellDocumentTag { get; }

		[Export ("checkSpellingOfString:startingAt:language:wrap:inSpellDocumentWithTag:wordCount:")]
		NSRange CheckSpelling (string stringToCheck, nint startingOffset, string language, bool wrapFlag, nint documentTag, out nint wordCount);

		[Export ("checkSpellingOfString:startingAt:")]
		NSRange CheckSpelling (string stringToCheck, nint startingOffset);

		[Export ("countWordsInString:language:")]
		nint CountWords (string stringToCount, string language);

		[Export ("checkGrammarOfString:startingAt:language:wrap:inSpellDocumentWithTag:details:")]
		NSRange CheckGrammar (string stringToCheck, int startingOffset, string language, bool wrapFlag, nint documentTag, NSDictionary[] details );

		[Export ("checkString:range:types:options:inSpellDocumentWithTag:orthography:wordCount:")]
		NSTextCheckingResult [] CheckString (string stringToCheck, NSRange range, NSTextCheckingTypes checkingTypes, NSDictionary options, nint tag, out NSOrthography orthography, out nint wordCount);

		//FIXME:
		//[Export ("requestCheckingOfString:range:types:options:inSpellDocumentWithTag:completionHandler:nintsequenceNumber,NSArray*results,NSOrthography*orthography,nintwordCount))completionHandler")]
		//int RequestChecking (string stringToCheck, NSRange range, NSTextCheckingTypes checkingTypes, NSDictionary options, int tag, IntPtr completionHandler );

		[Export ("menuForResult:string:options:atLocation:inView:")]
		NSMenu MenuForResults (NSTextCheckingResult result, string checkedString, NSDictionary options, CGPoint location, NSView view);

		[Export ("userQuotesArrayForLanguage:")]
		string [] UserQuotesArrayForLanguage (string language);

		[Export ("userReplacementsDictionary")]
		NSDictionary UserReplacementsDictionary { get; }

		[Export ("updateSpellingPanelWithMisspelledWord:")]
		void UpdateSpellingPanelWithMisspelledWord (string word);

		[Export ("updateSpellingPanelWithGrammarString:detail:")]
		void UpdateSpellingPanelWithGrammarl (string theString, NSDictionary detail);

		[Export ("spellingPanel")]
		NSPanel SpellingPanel { get; }

		[Export ("substitutionsPanel")]
		NSPanel SubstitutionsPanel { get; }

		[Export ("updatePanels")]
		void UpdatePanels ();

		[Export ("ignoreWord:inSpellDocumentWithTag:")]
		void IgnoreWord (string wordToIgnore, nint documentTag);

		[Export ("ignoredWordsInSpellDocumentWithTag:")]
		string [] IgnoredWords (nint documentTag);

		[Export ("setIgnoredWords:inSpellDocumentWithTag:")]
		void SetIgnoredWords (string [] words, nint documentTag);

		[Export ("guessesForWordRange:inString:language:inSpellDocumentWithTag:")]
		string [] GuessesForWordRange (NSRange range, string theString, string language, nint documentTag);

		[Export ("completionsForPartialWordRange:inString:language:inSpellDocumentWithTag:")]
		string [] CompletionsForPartialWordRange (NSRange range, string theString, string language, nint documentTag);

		[Export ("closeSpellDocumentWithTag:")]
		void CloseSpellDocument (nint documentTag);

		[Export ("availableLanguages")]
		string [] AvailableLanguages { get; }

		[Export ("userPreferredLanguages")]
		string [] UserPreferredLanguages { get; }

		[Export ("setWordFieldStringValue:")]
		void SetWordFieldStringValue (string aString);

		[Export ("learnWord:")]
		void LearnWord (string word);

		[Export ("hasLearnedWord:")]
		bool HasLearnedWord (string word);

		[Export ("unlearnWord:")]
		void UnlearnWord (string word);

		//Detected properties
		[Export ("accessoryView"), NullAllowed]
		NSView AccessoryView { get; set; }

		[Export ("substitutionsPanelAccessoryViewController")]
		NSViewController SubstitutionsPanelAccessoryViewController { get; set; }

		[Export ("automaticallyIdentifiesLanguages")]
		bool AutomaticallyIdentifiesLanguages { get; set; }

		[Export ("language"), Protected]
		string GetLanguage ();

		[Export ("setLanguage:"), Protected]
		bool SetLanguage (string language);
	}

	[BaseType (typeof (NSObject), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSSoundDelegate) })]
	[DisableDefaultCtor] // no valid handle is returned
	public interface NSSound {
		[Static]
		[Export ("soundNamed:")]
		NSSound FromName (string name);

		[Export ("initWithContentsOfURL:byReference:")]
		IntPtr Constructor (NSUrl url, bool byRef);

		[Export ("initWithContentsOfFile:byReference:")]
		IntPtr Constructor (string path, bool byRef);

		[Export ("initWithData:")]
		IntPtr Constructor (NSData data);

		[Static]
		[Export ("canInitWithPasteboard:")]
		bool CanCreateFromPasteboard (NSPasteboard pasteboard);

		[Static]
		[Export ("soundUnfilteredTypes")]
		string [] SoundUnfilteredTypes ();

		[Export ("initWithPasteboard:")]
		IntPtr Constructor (NSPasteboard pasteboard);

		[Export ("writeToPasteboard:")]
		void WriteToPasteboard (NSPasteboard pasteboard);

		[Export ("play")]
		bool Play ();

		[Export ("pause")]
		bool Pause ();

		[Export ("resume")]
		bool Resume ();

		[Export ("stop")]
		bool Stop ();

		[Export ("isPlaying")]
		bool IsPlaying ();

		[Export ("duration")]
		double Duration ();

		//Detected properties
		[Export ("name"), Protected]
		string GetName ();

		[Export ("setName:"), Protected]
		bool SetName (string name);

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSSoundDelegate Delegate { get; set; }

		[Export ("volume")]
		float Volume { get; set; } // 32-bit

		[Export ("currentTime")]
		double CurrentTime { get; set; }

		[Export ("loops")]
		bool Loops { get; set; }

		[Export ("playbackDeviceIdentifier")]
		string PlaybackDeviceID { get; set; }

		// FIXME: Poor docs, no type defined for the array elements
		[Export ("channelMapping")]
		NSObject ChannelMapping { get; set; }
	}

	[Model, BaseType (typeof (NSObject))]
	public interface NSSoundDelegate {
		[Export ("sound:didFinishPlaying:"), EventArgs ("NSSoundFinished")]
		void DidFinishPlaying (NSSound sound, bool finished);
	}

	[BaseType (typeof (NSView))]
	public partial interface NSSplitView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("drawDividerInRect:")]
		void DrawDivider (CGRect rect);

		[Export ("dividerColor")]
		NSColor DividerColor { get; }

		[Export ("dividerThickness")]
		nfloat DividerThickness { get; }

		[Export ("adjustSubviews")]
		void AdjustSubviews ();

		[Export ("isSubviewCollapsed:")]
		bool IsSubviewCollapsed (NSView subview);

		[Export ("minPossiblePositionOfDividerAtIndex:")]
		nfloat MinPositionOfDivider (nint dividerIndex);

		[Export ("maxPossiblePositionOfDividerAtIndex:")]
		nfloat MaxPositionOfDivider (nint dividerIndex);

		[Export ("setPosition:ofDividerAtIndex:")]
		void SetPositionOfDivider (nfloat position, nint dividerIndex);

		//Detected properties
		[Export ("vertical")]
		bool IsVertical { [Bind ("isVertical")]get; set; }

		[Export ("dividerStyle")]
		NSSplitViewDividerStyle DividerStyle { get; set; }

		[Export ("autosaveName")]
		string AutosaveName { get; set; }
		
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSSplitViewDelegate Delegate { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSSplitViewDelegate {
		[Export ("splitView:canCollapseSubview:") ] [DefaultValue (true)]
		bool CanCollapse (NSSplitView splitView, NSView subview);

		[Export ("splitView:shouldCollapseSubview:forDoubleClickOnDividerAtIndex:")] [DefaultValue (true)]
		bool ShouldCollapseForDoubleClick (NSSplitView splitView, NSView subview, nint doubleClickAtDividerIndex);

		[Export ("splitView:constrainMinCoordinate:ofSubviewAt:")]
		nfloat SetMinCoordinateOfSubview (NSSplitView splitView, nfloat proposedMinimumPosition, nint subviewDividerIndex);

		[Export ("splitView:constrainMaxCoordinate:ofSubviewAt:")]
		nfloat SetMaxCoordinateOfSubview (NSSplitView splitView, nfloat proposedMaximumPosition, nint subviewDividerIndex);

		[Export ("splitView:constrainSplitPosition:ofSubviewAt:")]
		nfloat ConstrainSplitPosition (NSSplitView splitView, nfloat proposedPosition, nint subviewDividerIndex);

		[Export ("splitView:resizeSubviewsWithOldSize:")]
		void Resize (NSSplitView splitView, CGSize oldSize);

		[Export ("splitView:shouldAdjustSizeOfSubview:")][DefaultValue (true)]
		bool ShouldAdjustSize (NSSplitView splitView, NSView view);

		[Export ("splitView:shouldHideDividerAtIndex:")] [DefaultValue (false)]
		bool ShouldHideDivider (NSSplitView splitView, nint dividerIndex);

		[Export ("splitView:effectiveRect:forDrawnRect:ofDividerAtIndex:")]
		CGRect GetEffectiveRect (NSSplitView splitView, CGRect proposedEffectiveRect, CGRect drawnRect, nint dividerIndex);

		[Export ("splitView:additionalEffectiveRectOfDividerAtIndex:")]
		CGRect GetAdditionalEffectiveRect (NSSplitView splitView, nint dividerIndex);

		[Export ("splitViewWillResizeSubviews:")]
		void SplitViewWillResizeSubviews (NSNotification notification);

		[Export ("splitViewDidResizeSubviews:")]
		void DidResizeSubviews (NSNotification notification);
	}

	[BaseType (typeof (NSObject))]
	public partial interface NSStatusBar {
		[Static, Export ("systemStatusBar")]
		NSStatusBar SystemStatusBar { get; }

		[Export ("statusItemWithLength:")]
		NSStatusItem CreateStatusItem (nfloat length);

		[Export ("removeStatusItem:")]
		void RemoveStatusItem (NSStatusItem item);

		[Export ("isVertical")]
		bool IsVertical { get; }

		[Export ("thickness")]
		nfloat Thickness { get; }
	}

	[BaseType (typeof (NSObject))]
	[PrivateDefaultCtor]
	public partial interface NSStatusItem {
		[Export ("statusBar")]
		NSStatusBar StatusBar { get; }

		// Since 12.10
		[Export("button")]
		NSStatusBarButton Button { get; }
		
		// Since 12.12
		[Export ("isVisible")]
		bool Visible { get; set; }

		[Export ("length")]
		nfloat Length { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use Button instead")]
		[Export ("action"), NullAllowed]
		Selector Action { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use Button instead")]
		[Export ("sendActionOn:")]
		nint SendActionOn (NSTouchPhase mask);

		[Export ("popUpStatusItemMenu:")]
		void PopUpStatusItemMenu (NSMenu menu);

		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use Button instead")]
		[Export ("drawStatusBarBackgroundInRect:withHighlight:")]
		void DrawStatusBarBackground (CGRect rect, bool highlight);

		//Detected properties
		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use Button instead")]
		[Export ("doubleAction")]
		Selector DoubleAction { get; set; }

		[Export ("target"), NullAllowed]
		NSObject Target { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use Button instead")]
		[Export ("title")]
		string Title { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use Button instead")]
		[Export ("attributedTitle")]
		NSAttributedString AttributedTitle { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use Button instead")]
		[Export ("image")]
		NSImage Image { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use Button instead")]
		[Export ("alternateImage")]
		NSImage AlternateImage { get; set; }

		[Export ("menu")]
		NSMenu Menu { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use Button instead")]
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")]get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use Button instead")]
		[Export ("toolTip")]
		string ToolTip { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use Button instead")]
		[Export ("highlightMode")]
		bool HighlightMode { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use Button instead")]
		[Export ("view")]
		NSView View { get; set; }

	}

	[BaseType(typeof(NSButton))]
	public interface NSStatusBarButton
	{
		[Export("appearsDisabled")]
		bool AppearsDisabled { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSShadow {
		[Export ("set")]
		void Set ();

		//Detected properties
		[Export ("shadowOffset")]
		CGSize ShadowOffset { get; set; }

		[Export ("shadowBlurRadius")]
		nfloat ShadowBlurRadius { get; set; }

		[Export ("shadowColor")]
		NSColor ShadowColor { get; set; }

	}

	//
	// Protocol, hence why it has no BaseType
	//
	public interface NSUserInterfaceItemIdentification {
		[Lion, Export ("identifier")]
		string Identifier { get; set; }
	}

	[Lion]
	[Model]
	partial interface NSTextFinderClient {
		[Abstract]
		[Export ("allowsMultipleSelection")]
		bool AllowsMultipleSelection { get;  }

		[Abstract]
		[Export ("editable")]
		bool Editable { [Bind ("isEditable")] get;  }

		[Abstract]
		[Export ("string")]
		string String { get;  }

		[Abstract]
		[Export ("firstSelectedRange")]
		NSRange FirstSelectedRange { get;  }

		[Abstract]
		[Export ("selectedRanges")]
		NSArray SelectedRanges { get; set;  }

		[Abstract]
		[Export ("visibleCharacterRanges")]
		NSArray VisibleCharacterRanges { get;  }

		[Abstract]
		[Export ("selectable")]
		bool Selectable { [Bind ("isSelectable")] get;  }

		[Abstract]
		[Export ("stringAtIndex:effectiveRange:endsWithSearchBoundary:")]
		string StringAtIndexeffectiveRangeendsWithSearchBoundary (nuint characterIndex, ref NSRange outRange, bool outFlag);

		[Abstract]
		[Export ("stringLength")]
		uint StringLength ();

		[Abstract]
		[Export ("scrollRangeToVisible:")]
		void ScrollRangeToVisible (NSRange range);

		[Abstract]
		[Export ("shouldReplaceCharactersInRanges:withStrings:")]
		bool ShouldReplaceCharactersInRangeswithStrings (NSArray ranges, NSArray strings);

		[Abstract]
		[Export ("replaceCharactersInRange:withString:")]
		void ReplaceCharactersInRangewithString (NSRange range, string str);

		[Abstract]
		[Export ("didReplaceCharacters")]
		void DidReplaceCharacters ();

		[Abstract]
		[Export ("contentViewAtIndex:effectiveCharacterRange:")]
		NSView ContentViewAtIndexeffectiveCharacterRange (nuint index, ref NSRange outRange);

		[Abstract]
		[Export ("rectsForCharacterRange:")]
		NSArray RectsForCharacterRange (NSRange range);

		[Abstract]
		[Export ("drawCharactersInRange:forContentView:")]
		void DrawCharactersInRangeforContentView (NSRange range, NSView view);
	}

 	public partial interface NSTextFinderBarContainer {
		[Abstract, Export ("findBarVisible"), Lion]
		bool FindBarVisible { [Bind ("isFindBarVisible")] get; set;  }

		[Abstract, Export ("findBarView"), Lion]
		NSView FindBarView { get; set; }

		[Abstract, Export ("findBarViewDidChangeHeight"), Lion]
		void FindBarViewDidChangeHeight ();

		/*[Abstract]
		[Export ("contentView")]
		NSView ContentView { get; }*/
	}

	[Lion]
	partial interface NSTextFinder {
		[Export ("client")]
		NSTextFinderClient Client { set; }

		[Export ("findBarContainer")]
		NSTextFinderBarContainer FindBarContainer { set; }

		[Export ("findIndicatorNeedsUpdate")]
		bool FindIndicatorNeedsUpdate { get; set; }

		[Export ("incrementalSearchingEnabled")]
		bool IncrementalSearchingEnabled { [Bind ("isIncrementalSearchingEnabled")] get; set;  }

		[Export ("incrementalMatchRanges")]
		NSArray IncrementalMatchRanges { get;  }

		[Export ("performAction:")]
		void PerformAction (NSTextFinderAction op);

		[Export ("validateAction:")]
		bool ValidateAction (NSTextFinderAction op);

		[Export ("cancelFindIndicator")]
		void CancelFindIndicator ();

		[Static]
		[Export ("drawIncrementalMatchHighlightInRect:")]
		void DrawIncrementalMatchHighlightInRect (CGRect rect);

		[Export ("noteClientStringWillChange")]
		void NoteClientStringWillChange ();
	}

	//64 bit reviewed
	[BaseType (typeof (NSResponder))]
	[Dispose ("__mt_tracking_var = null;")]
	public partial interface NSView : NSDraggingDestination, NSAnimatablePropertyContainer, NSUserInterfaceItemIdentification, NSAppearanceCustomization  {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("window")]
		NSWindow Window { get; }

		[Export ("superview")]
		NSView Superview { get; }

		[Export ("isDescendantOf:")]
		bool IsDescendantOf (NSView aView);

		[Export ("ancestorSharedWithView:")]
		NSView AncestorSharedWithView (NSView aView);

		[Export ("opaqueAncestor")]
		NSView OpaqueAncestor { get; }

		[Export ("isHiddenOrHasHiddenAncestor")]
		bool IsHiddenOrHasHiddenAncestor { get; }

		//[Export ("getRectsBeingDrawn:count:")]
		// void GetRectsBeingDrawn

		[Export ("needsToDrawRect:")]
		bool NeedsToDraw (CGRect aRect);

		[Export ("wantsDefaultClipping")]
		bool WantsDefaultClipping { get; }

		[Export ("viewDidHide")]
		void ViewDidHide ();

		[Export ("viewDidUnhide")]
		void ViewDidUnhide ();

		[Export ("addSubview:")][PostGet ("Subviews")]
		void AddSubview (NSView aView);

		[Export ("addSubview:positioned:relativeTo:")][PostGet ("Subviews")]
		void AddSubview (NSView aView, NSWindowOrderingMode place, [NullAllowed] NSView otherView);

		[Export ("viewWillMoveToWindow:")]
		void ViewWillMoveToWindow (NSWindow newWindow);

		[Export ("viewDidMoveToWindow")]
		void ViewDidMoveToWindow ();

		[Export ("viewWillMoveToSuperview:")]
		void ViewWillMoveToSuperview (NSView newSuperview);

		[Export ("viewDidMoveToSuperview")]
		void ViewDidMoveToSuperview ();

		[Export ("didAddSubview:")]
		void DidAddSubview (NSView subview);

		[Export ("willRemoveSubview:")]
		void WillRemoveSubview (NSView subview);

		[Export ("removeFromSuperview")]
		[PreSnippet ("var mySuper = Superview;")]
		[PostSnippet ("__mt_Superview_var = null;\n\tif (mySuper != null) {\n\t#pragma warning disable 168\n\tvar flush = mySuper.Subviews;\n#pragma warning restore 168\n\t}")]
		void RemoveFromSuperview ();

		[Export ("replaceSubview:with:")][PostGet ("Subviews")]
		void ReplaceSubviewWith (NSView oldView, NSView newView);

		[Export ("removeFromSuperviewWithoutNeedingDisplay")]
		[PreSnippet ("var mySuper = Superview;")]
		[PostSnippet ("__mt_Superview_var = null;\n\tif (mySuper != null) {\n\t#pragma warning disable 168\n\tvar flush = mySuper.Subviews;\n#pragma warning restore 168\n\t}")]
		void RemoveFromSuperviewWithoutNeedingDisplay ();

		[Export ("resizeSubviewsWithOldSize:")]
		void ResizeSubviewsWithOldSize (CGSize oldSize);

		[Export ("resizeWithOldSuperviewSize:")]
		void ResizeWithOldSuperviewSize (CGSize oldSize);

		[Export ("setFrameOrigin:")]
		void SetFrameOrigin (CGPoint newOrigin);

		[Export ("setFrameSize:")]
		void SetFrameSize (CGSize newSize);

		[Export ("setBoundsOrigin:")]
		void SetBoundsOrigin (CGPoint newOrigin);

		[Export ("setBoundsSize:")]
		void SetBoundsSize (CGSize newSize);

		[Export ("translateOriginToPoint:")]
		void TranslateOriginToPoint (CGPoint translation);

		[Export ("scaleUnitSquareToSize:")]
		void ScaleUnitSquareToSize (CGSize newUnitSize);

		[Export ("rotateByAngle:")]
		void RotateByAngle (nfloat angle);

		[Export ("isFlipped")]
		bool IsFlipped { get; }

		[Export ("isRotatedFromBase")]
		bool IsRotatedFromBase { get; }

		[Export ("isRotatedOrScaledFromBase")]
		bool IsRotatedOrScaledFromBase { get; }

		[Export ("isOpaque")]
		bool IsOpaque { get; }

		[Export ("convertPoint:fromView:")]
		CGPoint ConvertPointFromView (CGPoint aPoint, [NullAllowed] NSView aView);

		[Export ("convertPoint:toView:")]
		CGPoint ConvertPointToView (CGPoint aPoint, [NullAllowed] NSView aView);

		[Export ("convertSize:fromView:")]
		CGSize ConvertSizeFromView (CGSize aSize, [NullAllowed] NSView aView);

		[Export ("convertSize:toView:")]
		CGSize ConvertSizeToView (CGSize aSize, [NullAllowed] NSView aView);

		[Export ("convertRect:fromView:")]
		CGRect ConvertRectFromView (CGRect aRect, [NullAllowed] NSView aView);

		[Export ("convertRect:toView:")]
		CGRect ConvertRectToView (CGRect aRect, [NullAllowed] NSView aView);

		[Export ("centerScanRect:")]
		CGRect CenterScanRect (CGRect aRect);

		[Export ("convertPointToBase:")]
		CGPoint ConvertPointToBase (CGPoint aPoint);

		[Export ("convertPointFromBase:")]
		CGPoint ConvertPointFromBase (CGPoint aPoint);

		[Export ("convertSizeToBase:")]
		CGSize ConvertSizeToBase (CGSize aSize);

		[Export ("convertSizeFromBase:")]
		CGSize ConvertSizeFromBase (CGSize aSize);

		[Export ("convertRectToBase:")]
		CGRect ConvertRectToBase (CGRect aRect);

		[Export ("convertRectFromBase:")]
		CGRect ConvertRectFromBase (CGRect aRect);

		[Export ("canDraw")]
		bool CanDraw ();

		[Export ("setNeedsDisplayInRect:")]
		void SetNeedsDisplayInRect (CGRect invalidRect);

		//[Export ("setNeedsDisplay:")]
		//void SetNeedsDisplay (bool flag);
		
		[Export ("lockFocus")]
		void LockFocus ();

		[Export ("unlockFocus")]
		void UnlockFocus ();

		[Export ("lockFocusIfCanDraw")]
		bool LockFocusIfCanDraw ();

		[Export ("lockFocusIfCanDrawInContext:")]
		bool LockFocusIfCanDrawInContext (NSGraphicsContext context);

		[Export ("focusView")][Static]
		NSView FocusView ();

		[Export ("visibleRect")]
		CGRect VisibleRect ();

		[Export ("display")]
		void Display ();

		[Export ("displayIfNeeded")]
		void DisplayIfNeeded ();

		[Export ("displayIfNeededIgnoringOpacity")]
		void DisplayIfNeededIgnoringOpacity ();

		[Export ("displayRect:")]
		void DisplayRect (CGRect rect);

		[Export ("displayIfNeededInRect:")]
		void DisplayIfNeededInRect (CGRect rect);

		[Export ("displayRectIgnoringOpacity:")]
		void DisplayRectIgnoringOpacity (CGRect rect);

		[Export ("displayIfNeededInRectIgnoringOpacity:")]
		void DisplayIfNeededInRectIgnoringOpacity (CGRect rect);

		[Export ("drawRect:")]
		void DrawRect (CGRect dirtyRect);

		[Export ("displayRectIgnoringOpacity:inContext:")]
		void DisplayRectIgnoringOpacity (CGRect aRect, NSGraphicsContext context);

		[Export ("bitmapImageRepForCachingDisplayInRect:")]
		NSBitmapImageRep BitmapImageRepForCachingDisplayInRect (CGRect rect);

		[Export ("cacheDisplayInRect:toBitmapImageRep:")]
		void CacheDisplay (CGRect rect, NSBitmapImageRep bitmapImageRep);

		[Export ("viewWillDraw")]
		void ViewWillDraw ();

		[Export ("gState")]
		nint GState ();

		[Export ("allocateGState")]
		void AllocateGState ();

		[Export ("releaseGState")]
		void ReleaseGState ();

		[Export ("setUpGState")]
		void SetUpGState ();

		[Export ("renewGState")]
		void RenewGState ();

		[Export ("scrollPoint:")]
		void ScrollPoint (CGPoint aPoint);

		[Export ("scrollRectToVisible:")]
		bool ScrollRectToVisible (CGRect aRect);

		[Export ("autoscroll:")]
		bool Autoscroll (NSEvent theEvent);

		[Export ("adjustScroll:")]
		CGRect AdjustScroll (CGRect newVisible);

		[Export ("scrollRect:by:")]
		void ScrollRect (CGRect aRect, CGSize delta);

		[Export ("translateRectsNeedingDisplayInRect:by:")]
		void TranslateRectsNeedingDisplay (CGRect clipRect, CGSize delta);

		[Export ("hitTest:")]
		NSView HitTest (CGPoint aPoint);

		[Export ("mouse:inRect:")]
		bool IsMouseInRect (CGPoint aPoint, CGRect aRect);

		[Export ("viewWithTag:")]
		NSObject ViewWithTag (nint aTag);

		[Export ("tag")]
		nint Tag { get; }

		[Export ("performKeyEquivalent:")]
		bool PerformKeyEquivalent (NSEvent theEvent);

		[Export ("acceptsFirstMouse:")]
		bool AcceptsFirstMouse (NSEvent theEvent);

		[Export ("shouldDelayWindowOrderingForEvent:")]
		bool ShouldDelayWindowOrderingForEvent (NSEvent theEvent);

		[Export ("needsPanelToBecomeKey")]
		bool NeedsPanelToBecomeKey { get; }

		[Export ("mouseDownCanMoveWindow")]
		bool MouseDownCanMoveWindow { get; }

		[Export ("addCursorRect:cursor:")]
		void AddCursorRect (CGRect aRect, NSCursor cursor);

		[Export ("removeCursorRect:cursor:")]
		void RemoveCursorRect (CGRect aRect, NSCursor cursor);

		[Export ("discardCursorRects")]
		void DiscardCursorRects ();

		[Export ("resetCursorRects")]
		void ResetCursorRects ();

		[Export ("addTrackingRect:owner:userData:assumeInside:")]
		nint AddTrackingRect (CGRect aRect, NSObject anObject, IntPtr data, bool assumeInside);

		[Export ("removeTrackingRect:")]
		void RemoveTrackingRect (nint tag);

		[Export ("makeBackingLayer")]
		CALayer MakeBackingLayer ();

		[Export ("addTrackingArea:")][PostSnippet ("__mt_tracking_var = TrackingAreas ();")]
		void AddTrackingArea (NSTrackingArea trackingArea);

		[Export ("removeTrackingArea:")][PostSnippet ("__mt_tracking_var = TrackingAreas ();")]
		void RemoveTrackingArea (NSTrackingArea trackingArea);

		[Export ("trackingAreas")]
		NSTrackingArea [] TrackingAreas ();

		[Export ("updateTrackingAreas")]
		void UpdateTrackingAreas ();

		[Export ("shouldDrawColor")]
		bool ShouldDrawColor { get; }

		[Export ("enclosingScrollView")]
		NSScrollView EnclosingScrollView { get; }

		[Export ("menuForEvent:")]
		NSMenu MenuForEvent (NSEvent theEvent);

		[Static]
		[Export ("defaultMenu")]
		NSMenu DefaultMenu ();

		[Export ("addToolTipRect:owner:userData:")]
		nint AddToolTip (CGRect aRect, NSObject anObject, IntPtr data);

		[Export ("removeToolTip:")]
		void RemoveToolTip (nint tag);

		[Export ("removeAllToolTips")]
		void RemoveAllToolTips ();

		[Export ("viewWillStartLiveResize")]
		void ViewWillStartLiveResize ();

		[Export ("viewDidEndLiveResize")]
		void ViewDidEndLiveResize ();

		[Export ("inLiveResize")]
		bool InLiveResize { get; }

		[Export ("preservesContentDuringLiveResize")]
		bool PreservesContentDuringLiveResize { get; }

		[Export ("rectPreservedDuringLiveResize")]
		CGRect RectPreservedDuringLiveResize { get; }

		//[Export ("getRectsExposedDuringLiveResize:count:")]
		// void GetRectsExposedDuringLiveResizecount

		[Export ("inputContext")]
		NSTextInputContext InputContext { get; }

		//Detected properties
		[Export ("hidden")]
		bool Hidden { [Bind ("isHidden")]get; set; }

		[Export ("subviews")]
		NSView [] Subviews { get; set; }

		[Export ("postsFrameChangedNotifications")]
		bool PostsFrameChangedNotifications { get; set; }

		[Export ("autoresizesSubviews")]
		bool AutoresizesSubviews { get; set; }

		[Export ("autoresizingMask")]
		NSViewResizingMask AutoresizingMask { get; set; }

		[Export ("frame")]
		CGRect Frame { get; set; }

		[Export ("frameRotation")]
		nfloat FrameRotation { get; set; }

		[Export ("frameCenterRotation")]
		nfloat FrameCenterRotation { get; set; }

		[Export ("boundsRotation")]
		nfloat BoundsRotation { get; set; }

		[Export ("bounds")]
		CGRect Bounds { get; set; }

		[Export ("canDrawConcurrently")]
		bool CanDrawConcurrently { get; set; }

		[Export ("needsDisplay")]
		bool NeedsDisplay { get; set; }

		[Export ("acceptsTouchEvents")]
		bool AcceptsTouchEvents { get; set; }

		[Export ("wantsRestingTouches")]
		bool WantsRestingTouches { get; set; }

		[Export ("layerContentsRedrawPolicy")]
		NSViewLayerContentsRedrawPolicy LayerContentsRedrawPolicy { get; set; }

		[Export ("layerContentsPlacement")]
		NSViewLayerContentsPlacement LayerContentsPlacement { get; set; }

		[Export ("wantsLayer")]
		bool WantsLayer { get; set; }

		[Export ("layer"), NullAllowed]
		CALayer Layer { get; set; }

		[Export ("alphaValue")]
		nfloat AlphaValue { get; set; }

		[Export ("backgroundFilters"), NullAllowed]
		CIFilter [] BackgroundFilters { get; set; }

		[Export ("compositingFilter"), NullAllowed]
		CIFilter CompositingFilter { get; set; }

		[Export ("contentFilters"), NullAllowed]
		CIFilter [] ContentFilters { get; set; }

		[Export ("shadow")]
		NSShadow Shadow { get; set; }

		[Export ("postsBoundsChangedNotifications")]
		bool PostsBoundsChangedNotifications { get; set; }

		[Export ("toolTip")]
		string ToolTip { get; set; }
				
		[Export ("registerForDraggedTypes:")]
		void RegisterForDraggedTypes (string [] newTypes);

		[Export ("unregisterDraggedTypes")]
		void UnregisterDraggedTypes ();
		
		[Export ("registeredDraggedTypes")]
		string[] RegisteredDragTypes();

		[Export ("beginDraggingSessionWithItems:event:source:")]
		NSDraggingSession BeginDraggingSession (NSDraggingItem [] itmes, NSEvent evnt, NSDraggingSource source);

		[Export ("dragImage:at:offset:event:pasteboard:source:slideBack:")]
		void DragImage (NSImage anImage, CGPoint viewLocation, CGSize initialOffset, NSEvent theEvent, NSPasteboard pboard, NSObject sourceObj, bool slideFlag);

		[Export ("dragFile:fromRect:slideBack:event:")]
		bool DragFile (string filename, CGRect aRect, bool slideBack, NSEvent theEvent);

		[Export ("dragPromisedFilesOfTypes:fromRect:source:slideBack:event:")]
		bool DragPromisedFilesOfTypes (string[] typeArray, CGRect aRect, NSObject sourceObject, bool slideBack, NSEvent theEvent);

		[Export ("exitFullScreenModeWithOptions:")]
		void ExitFullscreenModeWithOptions(NSDictionary options);
		
		[Export ("enterFullScreenMode:withOptions:")]
		bool EnterFullscreenModeWithOptions(NSScreen screen, NSDictionary options);
		
		[Export ("isInFullScreenMode")]
		bool IsInFullscreenMode { get; }
		
		[Field ("NSFullScreenModeApplicationPresentationOptions")]   
		NSString NSFullScreenModeApplicationPresentationOptions { get; }
		
		// Fields
		[Field ("NSFullScreenModeAllScreens")]
		NSString NSFullScreenModeAllScreens { get; }
		
		[Field ("NSFullScreenModeSetting")]
		NSString NSFullScreenModeSetting { get; }
		
		[Field ("NSFullScreenModeWindowLevel")]
		NSString NSFullScreenModeWindowLevel { get; }

		[Notification, Field ("NSViewFrameDidChangeNotification")]
		NSString FrameChangedNotification { get; }
 
		[Notification, Field ("NSViewFocusDidChangeNotification")]
		NSString FocusChangedNotification { get; }

		[Notification, Field ("NSViewBoundsDidChangeNotification")]
		NSString BoundsChangedNotification { get; }

		[Notification, Field ("NSViewGlobalFrameDidChangeNotification")]
		NSString GlobalFrameChangedNotification { get; }

		[Notification, Field ("NSViewDidUpdateTrackingAreasNotification")]
		NSString UpdatedTrackingAreasNotification { get; }

		[Lion, Export ("constraints")]
		NSLayoutConstraint [] Constraints { get; }
		
		[Lion, Export ("addConstraint:")][PostGet ("Constraints")]
		void AddConstraint (NSLayoutConstraint constraint);

		[Lion, Export ("addConstraints:")][PostGet ("Constraints")]
		void AddConstraints (NSLayoutConstraint [] constraints);

		[Lion, Export ("removeConstraint:")][PostGet ("Constraints")]
		void RemoveConstraint (NSLayoutConstraint constraint);

		[Lion, Export ("removeConstraints:")][PostGet ("Constraints")]
		void RemoveConstraints (NSLayoutConstraint [] constraints);

		[Lion, Export ("layoutSubtreeIfNeeded")]
		void LayoutSubtreeIfNeeded ();

		[Lion, Export ("layout")]
		void Layout ();

		[Lion, Export ("needsUpdateConstraints")]
		bool NeedsUpdateConstraints { get; set; }

		[Lion, Export ("needsLayout")]
		bool NeedsLayout { get; set; }

		[Lion, Export ("updateConstraints")]
		void UpdateConstraints ();

		[Lion, Export ("updateConstraintsForSubtreeIfNeeded")]
		void UpdateConstraintsForSubtreeIfNeeded ();

		[Static]
		[Lion, Export ("requiresConstraintBasedLayout")]
		bool RequiresConstraintBasedLayout ();

		//Detected properties
		[Lion, Export ("translatesAutoresizingMaskIntoConstraints")]
		bool TranslatesAutoresizingMaskIntoConstraints { get; set; }

		[Lion, Export ("alignmentRectForFrame:")]
		CGRect GetAlignmentRectForFrame( CGRect frame );

		[Lion, Export ("frameForAlignmentRect:")]
		CGRect GetFrameForAlignmentRect (CGRect alignmentRect);

		[Lion, Export ("alignmentRectInsets")]
		NSEdgeInsets AlignmentRectInsets { get; }

		[Lion, Export ("baselineOffsetFromBottom")]
		nfloat BaselineOffsetFromBottom { get; }

		[Lion, Export ("intrinsicContentSize")]
		CGSize IntrinsicContentSize { get; }

		[Lion, Export ("invalidateIntrinsicContentSize")]
		void InvalidateIntrinsicContentSize ();

		[Lion, Export ("contentHuggingPriorityForOrientation:")]
		float GetContentHuggingPriorityForOrientation (NSLayoutConstraintOrientation orientation); // 32-bit

		[Lion, Export ("setContentHuggingPriority:forOrientation:")]
		void SetContentHuggingPriorityForOrientation (float priority, NSLayoutConstraintOrientation orientation); // 32-bit

		[Lion, Export ("contentCompressionResistancePriorityForOrientation:")]
		float GetContentCompressionResistancePriority (NSLayoutConstraintOrientation orientation); // 32-bit

		[Lion, Export ("setContentCompressionResistancePriority:forOrientation:")]
		void SetContentCompressionResistancePriority (float priority, NSLayoutConstraintOrientation orientation); // 32-bit

		[Lion, Export ("fittingSize")]
		CGSize FittingSize { get; }

		[Lion, Export ("constraintsAffectingLayoutForOrientation:")]
		NSLayoutConstraint [] GetConstraintsAffectingLayout (NSLayoutConstraintOrientation orientation);

		[Lion, Export ("hasAmbiguousLayout")]
		bool HasAmbiguousLayout { get; }

		[Lion, Export ("exerciseAmbiguityInLayout")]
		void ExerciseAmbiguityInLayout ();

		[Deprecated (PlatformName.MacOSX, 10, 8)]
		[Export ("performMnemonic:")]
		bool PerformMnemonic (string mnemonic);

		[Export ("nextKeyView"), NullAllowed]
		NSView NextKeyView { get; set; }

		[Export ("previousKeyView")]
		NSView PreviousKeyView { get; }

		[Export ("nextValidKeyView")]
		NSView NextValidKeyView { get; }

		[Export ("previousValidKeyView")]
		NSView PreviousValidKeyView { get; }

		[Export ("canBecomeKeyView")]
		bool CanBecomeKeyView { get; }

		[Export ("setKeyboardFocusRingNeedsDisplayInRect:")]
		void SetKeyboardFocusRingNeedsDisplay (CGRect rect);

		[Export ("focusRingType")]
		NSFocusRingType FocusRingType { get; set; }

		[Static, Export ("defaultFocusRingType")]
		NSFocusRingType DefaultFocusRingType { get; }

		[Export ("drawFocusRingMask")]
		void DrawFocusRingMask ();

		[Export ("focusRingMaskBounds")]
		CGRect FocusRingMaskBounds { get; }

		[Export ("noteFocusRingMaskChanged")]
		void NoteFocusRingMaskChanged ();

		[Lion, Export ("isDrawingFindIndicator")]
		bool IsDrawingFindIndicator { get; }
		
		[Export ("dataWithEPSInsideRect:")]
		NSData DataWithEpsInsideRect (CGRect rect);
	
		[Export ("dataWithPDFInsideRect:")]
		NSData DataWithPdfInsideRect (CGRect rect);
	
		[Export ("print:")]
		void Print (NSObject sender);
		
		[Export ("printJobTitle")]
		string PrintJobTitle { get; }
		
		[Export ("pageHeader")]
		NSAttributedString PageHeader { get; }

		[Export ("pageFooter")]
		NSAttributedString PageFooter { get; }

		[Export ("writeEPSInsideRect:toPasteboard:")]
		void WriteEpsInsideRect (CGRect rect, NSPasteboard pboard);

		[Export ("writePDFInsideRect:toPasteboard:")]
		void WritePdfInsideRect (CGRect rect, NSPasteboard pboard);

		[Export ("drawPageBorderWithSize:")]
		void DrawPageBorder (CGSize borderSize);

		[Export ("drawSheetBorderWithSize:")]
		void DrawSheetBorder (CGSize borderSize);

		[Export ("heightAdjustLimit")]
		nfloat HeightAdjustLimit { get; }

		[Export ("widthAdjustLimit")]
		nfloat WidthAdjustLimit { get; }

		[Export ("adjustPageWidthNew:left:right:limit:")]
		void AdjustPageWidthNew (ref nfloat newRight, nfloat left, nfloat proposedRight, nfloat rightLimit);

		[Export ("adjustPageHeightNew:top:bottom:limit:")]
		void AdjustPageHeightNew (ref nfloat newBottom, nfloat top, nfloat proposedBottom, nfloat bottomLimit);

		[Export ("knowsPageRange:")]
		bool KnowsPageRange (ref NSRange aRange);
		
		[Export ("rectForPage:")]
		//TODO: figure out size of NSIndex - assuming nint
		CGRect RectForPage (nint pageNumber);

		[Export ("locationOfPrintRect:")]
		CGPoint LocationOfPrintRect (CGRect aRect);

		[Lion, Export ("wantsBestResolutionOpenGLSurface")]
		bool WantsBestResolutionOpenGLSurface { get; set; }

		[Lion, Export ("backingAlignedRect:options:")]
		CGRect BackingAlignedRect (CGRect aRect, NSAlignmentOptions options);

		[Lion, Export ("convertRectFromBacking:")]
		CGRect ConvertRectFromBacking (CGRect aRect);

		[Lion, Export ("convertRectToBacking:")]
		CGRect ConvertRectToBacking (CGRect aRect);

		[Lion, Export ("convertRectFromLayer:")]
		CGRect ConvertRectFromLayer (CGRect aRect);

		[Lion, Export ("convertRectToLayer:")]
		CGRect ConvertRectToLayer (CGRect aRect);

		[Lion, Export ("convertPointFromBacking:")]
		CGPoint ConvertPointFromBacking (CGPoint aPoint);

		[Lion, Export ("convertPointToBacking:")]
		CGPoint ConvertPointToBacking (CGPoint aPoint);

		[Lion, Export ("convertPointFromLayer:")]
		CGPoint ConvertPointFromLayer (CGPoint aPoint);

		[Lion, Export ("convertPointToLayer:")]
		CGPoint ConvertPointToLayer (CGPoint aPoint);

		[Lion, Export ("convertSizeFromBacking:")]
		CGSize ConvertSizeFromBacking (CGSize aSize);

		[Lion, Export ("convertSizeToBacking:")]
		CGSize ConvertSizeToBacking (CGSize aSize);

		[Lion, Export ("convertSizeFromLayer:")]
		CGSize ConvertSizeFromLayer (CGSize aSize);

		[Lion, Export ("convertSizeToLayer:")]
		CGSize ConvertSizeToLayer (CGSize aSize);
		
		[Export ("viewDidChangeBackingProperties")]
		void DidChangeBackingProperties ();

		[Mac (10,10)]
		[Export ("allowsVibrancy")]
		bool AllowsVibrancy { get; }

		[Mac (10,10)]
		[Export ("gestureRecognizers", ArgumentSemantic.Copy)]
		NSGestureRecognizer [] GestureRecognizers { get; set; }

		[Mac (10,10)]
		[Export ("addGestureRecognizer:")]
		void AddGestureRecognizer (NSGestureRecognizer gestureRecognizer);

		[Mac (10,10)]
		[Export ("removeGestureRecognizer:")]
		void RemoveGestureRecognizer (NSGestureRecognizer gestureRecognizer);

		[Export ("prepareForReuse")]
		void PrepareForReuse ();

		[Mac (10,9)]
		[Static, Export ("isCompatibleWithResponsiveScrolling")]
		bool IsCompatibleWithResponsiveScrolling { get; }

		[Mac (10,9)]
		[Export ("prepareContentInRect:")]
		void PrepareContentInRect (CGRect rect);
		
		[Mac (10,9)]
		[Export ("canDrawSubviewsIntoLayer")]
		bool CanDrawSubviewsIntoLayer { get; set; }

		[Mac (10,9)]
		[Export ("layerUsesCoreImageFilters")]
		bool LayerUsesCoreImageFilters { get; set; }

		[Mac (10,9)] // NS_AVAILABLE_MAC(10,8); but the 10.8 headers do not contain this member, nor can the dontlink tests find it in 10.8.
		[Export ("userInterfaceLayoutDirection")]
		NSUserInterfaceLayoutDirection UserInterfaceLayoutDirection { get; set; }

		[Mac (10,9)]
		[Export ("preparedContentRect")]
		CGRect PreparedContentRect { get; set; }

		// [Mac (10,11)]
		// [Export ("pressureConfiguration", ArgumentSemantic.Strong)]
		// NSPressureConfiguration PressureConfiguration { get; set; }

		[Mac (10,11)]
		[Export ("willOpenMenu:withEvent:")]
		void WillOpenMenu (NSMenu menu, NSEvent theEvent);

		[Mac (10,11)]
		[Export ("didCloseMenu:withEvent:")]
		void DidCloseMenu (NSMenu menu, [NullAllowed] NSEvent theEvent);

		// NSConstraintBasedLayoutCoreMethods

		// [Mac (10,11)]
		// [Export ("leadingAnchor", ArgumentSemantic.Strong)]
		// NSLayoutXAxisAnchor LeadingAnchor { get; }

		// [Mac (10,11)]
		// [Export ("trailingAnchor", ArgumentSemantic.Strong)]
		// NSLayoutXAxisAnchor TrailingAnchor { get; }

		// [Mac (10,11)]
		// [Export ("leftAnchor", ArgumentSemantic.Strong)]
		// NSLayoutXAxisAnchor LeftAnchor { get; }

		// [Mac (10,11)]
		// [Export ("rightAnchor", ArgumentSemantic.Strong)]
		// NSLayoutXAxisAnchor RightAnchor { get; }

		// [Mac (10,11)]
		// [Export ("topAnchor", ArgumentSemantic.Strong)]
		// NSLayoutYAxisAnchor TopAnchor { get; }

		// [Mac (10,11)]
		// [Export ("bottomAnchor", ArgumentSemantic.Strong)]
		// NSLayoutYAxisAnchor BottomAnchor { get; }

		// [Mac (10,11)]
		// [Export ("widthAnchor", ArgumentSemantic.Strong)]
		// NSLayoutDimension WidthAnchor { get; }

		// [Mac (10,11)]
		// [Export ("heightAnchor", ArgumentSemantic.Strong)]
		// NSLayoutDimension HeightAnchor { get; }

		// [Mac (10,11)]
		// [Export ("centerXAnchor", ArgumentSemantic.Strong)]
		// NSLayoutXAxisAnchor CenterXAnchor { get; }

		// [Mac (10,11)]
		// [Export ("centerYAnchor", ArgumentSemantic.Strong)]
		// NSLayoutYAxisAnchor CenterYAnchor { get; }

		// [Mac (10,11)]
		// [Export ("firstBaselineAnchor", ArgumentSemantic.Strong)]
		// NSLayoutYAxisAnchor FirstBaselineAnchor { get; }

		// [Mac (10,11)]
		// [Export ("lastBaselineAnchor", ArgumentSemantic.Strong)]
		// NSLayoutYAxisAnchor LastBaselineAnchor { get; }

		[Mac (10,11)]
		[Export ("firstBaselineOffsetFromTop")]
		nfloat FirstBaselineOffsetFromTop { get; }

		[Mac (10,11)]
		[Export ("lastBaselineOffsetFromBottom")]
		nfloat LastBaselineOffsetFromBottom { get; }

		[Mac (10,11)]
		[Field ("NSViewNoIntrinsicMetric")]
		nfloat NoIntrinsicMetric { get; }

		// [Mac (10,11)]
		// [Export ("addLayoutGuide:")]
		// void AddLayoutGuide (NSLayoutGuide guide);

		// [Mac (10,11)]
		// [Export ("removeLayoutGuide:")]
		// void RemoveLayoutGuide (NSLayoutGuide guide);

		// [Mac (10,11)]
		// [Export ("layoutGuides", ArgumentSemantic.Copy)]
		// NSLayoutGuide[] LayoutGuides { get; }

		[Mac (10,14)]
		[Export ("viewDidChangeEffectiveAppearance")]
		void ViewDidChangeEffectiveAppearance ();

		[Internal]
		[Export ("sortSubviewsUsingFunction:context:")]
		void SortSubviews (IntPtr function_pointer, IntPtr context);

		[Mac (10, 15)]
		[Export ("horizontalContentSizeConstraintActive")]
		bool HorizontalContentSizeConstraintActive { [Bind ("isHorizontalContentSizeConstraintActive")] get; set; }

		[Mac (10, 15)]
		[Export ("verticalContentSizeConstraintActive")]
		bool VerticalContentSizeConstraintActive { [Bind ("isVerticalContentSizeConstraintActive")] get; set; }

		[Mac (11, 0)]
		[Export ("safeAreaInsets")]
		NSEdgeInsets SafeAreaInsets { get; }

		[Mac (11, 0)]
		[Export ("additionalSafeAreaInsets", ArgumentSemantic.Assign)]
		NSEdgeInsets AdditionalSafeAreaInsets { get; set; }

		// [Mac (11, 0)]
		// [Export ("safeAreaLayoutGuide", ArgumentSemantic.Strong)]
		// NSLayoutGuide SafeAreaLayoutGuide { get; }

		[Mac (11, 0)]
		[Export ("safeAreaRect")]
		CGRect SafeAreaRect { get; }

		// [Mac (11, 0)]
		// [Export ("layoutMarginsGuide", ArgumentSemantic.Strong)]
		// NSLayoutGuide LayoutMarginsGuide { get; }
	}

	//64 bit reviewed
	[BaseType (typeof (NSAnimation))]
	public interface NSViewAnimation { 
		[Export ("initWithViewAnimations:")]
		IntPtr Constructor (NSDictionary [] viewAnimations);
	
		[Export ("viewAnimations")]
		NSDictionary [] ViewAnimations { get; set; }
	
		[Export ("animator")]
		NSObject Animator { [return: Proxy] get; }
	
		[Export ("animations")]
		NSDictionary Animations  { get; set; }
	
		[Export ("animationForKey:")]
		NSObject AnimationForKey (string  key);
	
		[Static]
		[Export ("defaultAnimationForKey:")]
		NSObject DefaultAnimationForKey (string  key);
	
		[Field ("NSViewAnimationTargetKey")]
		NSString TargetKey { get; }
		
		[Field ("NSViewAnimationStartFrameKey")]
		NSString StartFrameKey { get; }
		
		[Field ("NSViewAnimationEndFrameKey")]
		NSString EndFrameKey { get; }
		
		[Field ("NSViewAnimationEffectKey")]
		NSString EffectKey { get; }
		
		[Field ("NSViewAnimationFadeInEffect")]
		NSString FadeInEffect { get; }
		
		[Field ("NSViewAnimationFadeOutEffect")]
		NSString FadeOutEffect { get; }
	}
	

	[BaseType (typeof (NSResponder))]
	public interface NSViewController {
		[Export ("initWithNibName:bundle:")]
		IntPtr Constructor ([NullAllowed] string nibNameOrNull, [NullAllowed] NSBundle nibBundleOrNull);

		[Export ("loadView")]
		void LoadView ();

		[Export ("nibName")]
		string NibName { get; }

		[Export ("nibBundle")]
		NSBundle NibBundle { get; }

		[Export ("commitEditingWithDelegate:didCommitSelector:contextInfo:")]
		void CommitEditing (NSObject delegateObject, Selector didCommitSelector, IntPtr contextInfo);

		[Export ("commitEditing")]
		bool CommitEditing ();

		[Export ("discardEditing")]
		void DiscardEditing ();

		//Detected properties
		[Export ("representedObject")]
		NSObject RepresentedObject { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("view")]
		NSView View { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface NSTableColumn : NSUserInterfaceItemIdentification {
		[Lion, Export ("initWithIdentifier:")]
		IntPtr Constructor (string identifier);

		[Lion, Export ("initWithIdentifier:")]
		IntPtr Constructor (NSString identifier);

		[Obsolete, Export ("initWithIdentifier:")]
		IntPtr Constructor (NSObject identifier);
	
		[Export ("dataCellForRow:")]
		NSCell DataCellForRow (nint row);
		
		[Export ("sizeToFit")]
		void SizeToFit ();
		
		[Export ("tableView")]
		NSTableView TableView { get; set; }
		
		[Export ("width")]
		nfloat Width { get; set; }
		
		[Export ("minWidth")]
		nfloat MinWidth { get; set; }
		
		[Export ("maxWidth")]
		nfloat MaxWidth { get; set; }
	
		[Export ("headerCell")]
		NSCell HeaderCell { get; set; }

		[Export ("dataCell")]
		NSCell DataCell { get; set; }
	
		[Export ("editable")]
		bool Editable { [Bind ("isEditable")]get; set; }
	
		[Export ("sortDescriptorPrototype"), NullAllowed]
		NSSortDescriptor SortDescriptorPrototype { get; set; }
	
		[Export ("resizingMask")]
		NSTableColumnResizing ResizingMask { get; set; }
	
		[Export ("headerToolTip")]
		string HeaderToolTip { get; set; }
	
		[Export ("hidden")]
		bool Hidden { [Bind ("isHidden")]get; set; }
	}

	[Lion]
	[BaseType (typeof (NSView))]
	public interface NSTableRowView {
		[Export ("selectionHighlightStyle")]
		NSTableViewSelectionHighlightStyle SelectionHighlightStyle { get; set;  }

		[Export ("emphasized")]
		bool Emphasized { [Bind ("isEmphasized")] get; set;  }

		[Export ("groupRowStyle")]
		bool GroupRowStyle { [Bind ("isGroupRowStyle")] get; set;  }

		[Export ("selected")]
		bool Selected { [Bind ("isSelected")] get; set;  }

		[Export ("floating")]
		bool Floating { [Bind ("isFloating")] get; set;  }

		[Export ("draggingDestinationFeedbackStyle")]
		NSTableViewDraggingDestinationFeedbackStyle DraggingDestinationFeedbackStyle { get; set;  }

		[Export ("indentationForDropOperation")]
		nfloat IndentationForDropOperation { get; set;  }

		[Export ("interiorBackgroundStyle")]
		NSBackgroundStyle InteriorBackgroundStyle { get;  }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set;  }

		[Export ("numberOfColumns")]
		nint NumberOfColumns { get;  }

		[Export ("targetForDropOperation")]
		bool TargetForDropOperation { [Bind ("isTargetForDropOperation")] get; set; }

		[Export ("drawBackgroundInRect:")]
		void DrawBackground (CGRect dirtyRect);

		[Export ("drawSelectionInRect:")]
		void DrawSelection (CGRect dirtyRect);

		[Export ("drawSeparatorInRect:")]
		void DrawSeparator (CGRect dirtyRect);

		[Export ("drawDraggingDestinationFeedbackInRect:")]
		void DrawDraggingDestinationFeedback (CGRect dirtyRect);

		[Export ("viewAtColumn:")]
		NSView ViewAtColumn (nint column);
	}

	[Lion]
	[BaseType (typeof (NSView))]
	public partial interface NSTableCellView {
		[Export ("backgroundStyle")]
		NSBackgroundStyle BackgroundStyle {
			get; set;
		}

		[Export ("imageView", ArgumentSemantic.Assign)]
		NSImageView ImageView {
			get; set;
		}

		[Export ("objectValue", ArgumentSemantic.Retain)]
		NSObject ObjectValue {
			get; set;
		}

		[Export ("rowSizeStyle")]
		NSTableViewRowSizeStyle RowSizeStyle {
			get; set;
		}

		[Export ("textField", ArgumentSemantic.Assign)]
		NSTextField TextField {
			get; set;
		}

		[Export ("draggingImageComponents", ArgumentSemantic.Retain)]
		NSArray DraggingImageComponents {
			get;
		}
	}

	public delegate void NSTableViewRowHandler (NSTableRowView rowView, int row);
	//64 bit reviewed
	[BaseType (typeof (NSControl), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTableViewDelegate)})]
	public partial interface NSTableView : NSDraggingSource {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("noteHeightOfRowsWithIndexesChanged:")]
		void NoteHeightOfRowsWithIndexesChanged (NSIndexSet indexSet );
	
		[Export ("tableColumns")]
		NSTableColumn[] TableColumns ();
	
		[Export ("numberOfColumns")]
		nint ColumnCount { get; }
	
		[Export ("numberOfRows")]
		nint RowCount { get; }
	
		[Export ("addTableColumn:")]
		void AddColumn (NSTableColumn tableColumn);
	
		[Export ("removeTableColumn:")]
		void RemoveColumn (NSTableColumn tableColumn);
	
		[Export ("moveColumn:toColumn:")]
		void MoveColumn (nint oldIndex, nint newIndex);
	
		[Export ("columnWithIdentifier:")]
		nint FindColumn (NSString identifier);
	
		[Export ("tableColumnWithIdentifier:")]
		NSTableColumn FindTableColumn (NSString identifier);
	
		[Export ("tile")]
		void Tile ();
	
		[Export ("sizeToFit")]
		void SizeToFit ();
	
		[Export ("sizeLastColumnToFit")]
		void SizeLastColumnToFit ();
	
		[Export ("scrollRowToVisible:")]
		void ScrollRowToVisible (nint row);
	
		[Export ("scrollColumnToVisible:")]
		void ScrollColumnToVisible (nint column);
	
		[Export ("reloadData")]
		void ReloadData ();
	
		[Export ("noteNumberOfRowsChanged")]
		void NoteNumberOfRowsChanged ();
	
		[Export ("reloadDataForRowIndexes:columnIndexes:")]
		void ReloadData (NSIndexSet rowIndexes, NSIndexSet columnIndexes );
	
		[Export ("editedColumn")]
		nint EditedColumn { get; }
	
		[Export ("editedRow")]
		nint EditedRow { get; }
	
		[Export ("clickedColumn")]
		nint ClickedColumn { get; }
	
		[Export ("clickedRow")]
		nint ClickedRow { get; }
	
		[Export ("setIndicatorImage:inTableColumn:")]
		void SetIndicatorImage ([NullAllowed] NSImage anImage, NSTableColumn tableColumn);
	
		[Export ("indicatorImageInTableColumn:")]
		NSImage GetIndicatorImage (NSTableColumn tableColumn);
	
		[Export ("canDragRowsWithIndexes:atPoint:")]
		bool CanDragRows (NSIndexSet rowIndexes, CGPoint mouseDownPoint );
	
		// FIXME: binding, CGPointPointer
		//[Export ("dragImageForRowsWithIndexes:tableColumns:event:offset:")]
		//NSImage DragImageForRowsWithIndexestableColumnseventoffset (NSIndexSet dragRows, NSArray tableColumns, NSEvent dragEvent, CGPointPointer dragImageOffset );
	
		[Export ("setDraggingSourceOperationMask:forLocal:")]
		void SetDraggingSourceOperationMask (NSDragOperation mask, bool isLocal);
	
		[Export ("setDropRow:dropOperation:")]
		void SetDropRowDropOperation (nint row, NSTableViewDropOperation dropOperation);
	
		[Export ("selectAll:")]
		void SelectAll ([NullAllowed] NSObject sender);
	
		[Export ("deselectAll:")]
		void DeselectAll ([NullAllowed] NSObject sender);
	
		[Export ("selectColumnIndexes:byExtendingSelection:")]
		void SelectColumns (NSIndexSet indexes, bool byExtendingSelection);
	
		[Export ("selectRowIndexes:byExtendingSelection:")]
		void SelectRows (NSIndexSet indexes, bool byExtendingSelection);
	
		[Export ("selectedColumnIndexes")]
		NSIndexSet SelectedColumns { get; }
	
		[Export ("selectedRowIndexes")]
		NSIndexSet SelectedRows { get; }
	
		[Export ("deselectColumn:")]
		void DeselectColumn (nint column);
	
		[Export ("deselectRow:")]
		void DeselectRow (nint row);
	
		[Export ("selectedColumn")]
		int SelectedColumn { get; }
	
		[Export ("selectedRow")]
		nint SelectedRow { get; }
	
		[Export ("isColumnSelected:")]
		bool IsColumnSelected (nint column);
	
		[Export ("isRowSelected:")]
		bool IsRowSelected (nint row);
	
		[Export ("numberOfSelectedColumns")]
		nint SelectedColumnsCount { get; }
	
		[Export ("numberOfSelectedRows")]
		nint SelectedRowCount { get; }
	
		[Export ("rectOfColumn:")]
		CGRect RectForColumn (nint column);
	
		[Export ("rectOfRow:")]
		CGRect RectForRow (nint row);
	
		[Export ("columnIndexesInRect:")]
		NSIndexSet GetColumnIndexesInRect (CGRect rect);
	
		[Export ("rowsInRect:")]
		NSRange RowsInRect (CGRect rect);
	
		[Export ("columnAtPoint:")]
		nint GetColumn (CGPoint point);
	
		[Export ("rowAtPoint:")]
		nint GetRow (CGPoint point);
	
		[Export ("frameOfCellAtColumn:row:")]
		CGRect GetCellFrame (nint column, nint row);
	
		[Export ("preparedCellAtColumn:row:")]
		NSCell GetCell (nint column, nint row );
	
		[Export ("textShouldBeginEditing:")]
		bool TextShouldBeginEditing (NSText textObject);
	
		[Export ("textShouldEndEditing:")]
		bool TextShouldEndEditing (NSText textObject);
	
		[Export ("textDidBeginEditing:")]
		void TextDidBeginEditing (NSNotification notification);
	
		[Export ("textDidEndEditing:")]
		void TextDidEndEditing (NSNotification notification);
	
		[Export ("textDidChange:")]
		void TextDidChange (NSNotification notification);
	
		[Export ("shouldFocusCell:atColumn:row:")]
		bool ShouldFocusCell (NSCell cell, nint column, nint row );
	
		[Export ("performClickOnCellAtColumn:row:")]
		void PerformClick (nint column, nint row );
	
		[Export ("editColumn:row:withEvent:select:")]
		void EditColumn (nint column, nint row, NSEvent theEvent, bool select);
	
		[Export ("drawRow:clipRect:")]
		void DrawRow (nint row, CGRect clipRect);
	
		[Export ("highlightSelectionInClipRect:")]
		void HighlightSelection (CGRect clipRect);
	
		[Export ("drawGridInClipRect:")]
		void DrawGrid (CGRect clipRect);
	
		[Export ("drawBackgroundInClipRect:")]
		void DrawBackground (CGRect clipRect );
	
		//Detected properties
		[Export ("dataSource")][NullAllowed]
		NSObject WeakDataSource { get; set; }

		[Wrap ("WeakDataSource")]
		NSTableViewDataSource DataSource { get; set; }
	
		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }
	
		[Wrap ("WeakDelegate")][NullAllowed]
		NSTableViewDelegate Delegate { get; set; }
	
		[Export ("headerView"), NullAllowed]
		NSTableHeaderView HeaderView { get; set; }
	
		[Export ("cornerView")]
		NSView CornerView { get; set; }
	
		[Export ("allowsColumnReordering")]
		bool AllowsColumnReordering { get; set; }
	
		[Export ("allowsColumnResizing")]
		bool AllowsColumnResizing { get; set; }
	
		[Export ("columnAutoresizingStyle")]
		NSTableViewColumnAutoresizingStyle ColumnAutoresizingStyle { get; set; }
	
		[Export ("gridStyleMask")]
		NSTableViewGridStyle GridStyleMask { get; set; }
	
		[Export ("intercellSpacing")]
		CGSize IntercellSpacing { get; set; }
	
		[Export ("usesAlternatingRowBackgroundColors")]
		bool UsesAlternatingRowBackgroundColors { get; set; }
	
		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }
	
		[Export ("gridColor")]
		NSColor GridColor { get; set; }
	
		[Export ("rowHeight")]
		nfloat RowHeight { get; set; }
	
		[Export ("doubleAction")]
		Selector DoubleAction { get; set; }
	
		[Export ("sortDescriptors")]
		NSSortDescriptor[] SortDescriptors { get; set; }
	
		[Export ("highlightedTableColumn")]
		NSTableColumn HighlightedTableColumn { get; set; }
	
		[Export ("verticalMotionCanBeginDrag")]
		bool VerticalMotionCanBeginDrag { get; set; }
	
		[Export ("allowsMultipleSelection")]
		bool AllowsMultipleSelection { get; set; }
	
		[Export ("allowsEmptySelection")]
		bool AllowsEmptySelection { get; set; }
	
		[Export ("allowsColumnSelection")]
		bool AllowsColumnSelection { get; set; }
	
		[Export ("allowsTypeSelect")]
		bool AllowsTypeSelect { get; set; }
	
		[Export ("selectionHighlightStyle")]
		NSTableViewSelectionHighlightStyle SelectionHighlightStyle { get; set; }
	
		[Export ("draggingDestinationFeedbackStyle")]
		NSTableViewDraggingDestinationFeedbackStyle DraggingDestinationFeedbackStyle { get; set; }
	
		[Export ("autosaveName")]
		string AutosaveName { get; set; }
	
		[Export ("autosaveTableColumns")]
		bool AutosaveTableColumns { get; set; }
	
		[Export ("focusedColumn")]
		nint FocusedColumn { get; set; }

		[Lion]
		[Export ("effectiveRowSizeStyle")]
		NSTableViewRowSizeStyle EffectiveRowSizeStyle { get; }

		[Lion]
		[Export ("viewAtColumn:row:makeIfNecessary:")]
		NSView GetView (nint column, nint row, bool makeIfNecessary);

		[Lion]
		[Export ("rowViewAtRow:makeIfNecessary:")]
		NSTableRowView GetRowView (nint row, bool makeIfNecessary);

		[Lion]
		[Export ("rowForView:")]
		nint RowForView (NSView view);

		[Lion]
		[Export ("columnForView:")]
		nint ColumnForView (NSView view);

		[Lion]
		[Export ("makeViewWithIdentifier:owner:")]
		NSView MakeView (string identifier, NSObject owner);

		[Lion]
		[Export ("enumerateAvailableRowViewsUsingBlock:")]
		void EnumerateAvailableRowViews (NSTableViewRowHandler callback);

		[Lion]
		[Export ("beginUpdates")]
		void BeginUpdates ();

		[Lion]
		[Export ("endUpdates")]
		void EndUpdates ();

		[Lion]
		[Export ("insertRowsAtIndexes:withAnimation:")]
		void InsertRows (NSIndexSet indexes, NSTableViewAnimation animationOptions);

		[Lion]
		[Export ("removeRowsAtIndexes:withAnimation:")]
		void RemoveRows (NSIndexSet indexes, NSTableViewAnimation animationOptions);

		[Lion]
		[Export ("moveRowAtIndex:toIndex:")]
		void MoveRow (nint oldIndex, nint newIndex);

		[Lion]
		[Export ("rowSizeStyle")]
		NSTableViewRowSizeStyle RowSizeStyle { get; set; }

		[Lion]
		[Export ("floatsGroupRows")]
		bool FloatsGroupRows { get; set; }

		[Field ("NSTableViewRowViewKey")]
		NSString RowViewKey { get; }

		[Export ("registerNib:forIdentifier:")]
		void RegisterNib ([NullAllowed] NSNib nib, string identifier);

		[Export ("didAddRowView:forRow:")]
		void RowViewAdded (NSTableRowView rowView, nint row);

		[Export ("didRemoveRowView:forRow:")]
		void RowViewRemoved (NSTableRowView rowView, nint row);

		[Export ("registeredNibsByIdentifier", ArgumentSemantic.Copy)]
		NSDictionary RegisteredNibsByIdentifier { get; }

		[Mac (10,10)]
		[Export ("usesStaticContents")]
		bool UsesStaticContents { get; set; }		

		[Mac (10,11)]
		[Export ("hideRowsAtIndexes:withAnimation:")]
		void HideRows (NSIndexSet indexes, NSTableViewAnimation rowAnimation);

		[Mac (10,11)]
		[Export ("unhideRowsAtIndexes:withAnimation:")]
		void UnhideRows (NSIndexSet indexes, NSTableViewAnimation rowAnimation);

		[Mac (10,11)]
		[Export ("hiddenRowIndexes", ArgumentSemantic.Copy)]
		NSIndexSet HiddenRowIndexes { get; }

		[Mac (10,11)]
		[Export ("rowActionsVisible")]
		bool RowActionsVisible { get; set; }

		[Mac (10,12)]
		[Export ("userInterfaceLayoutDirection")]
		NSUserInterfaceLayoutDirection UserInterfaceLayoutDirection { get; set; }

		[Mac (10, 13)]
		[Export ("usesAutomaticRowHeights")]
		bool UsesAutomaticRowHeights { get; set; }

		[Mac (11, 0)]
		[Export ("style", ArgumentSemantic.Assign)]
		NSTableViewStyle Style { get; set; }

		[Mac (11, 0)]
		[Export ("effectiveStyle")]
		NSTableViewStyle EffectiveStyle { get; }
	} 

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	[Model]
	public partial interface NSTableViewDelegate {
		[Export ("tableView:willDisplayCell:forTableColumn:row:"), EventArgs ("NSTableViewCell")]
		void WillDisplayCell (NSTableView tableView, NSObject cell, NSTableColumn tableColumn, nint row);
	
		[Export ("tableView:shouldEditTableColumn:row:"), DelegateName ("NSTableViewColumnRowPredicate"), DefaultValue (false)]
		bool ShouldEditTableColumn (NSTableView tableView, NSTableColumn tableColumn, nint row);
	
		[Export ("selectionShouldChangeInTableView:"), DelegateName ("NSTableViewPredicate"), DefaultValue (false)]
		bool SelectionShouldChange (NSTableView tableView);
	
		[Export ("tableView:shouldSelectRow:"), DelegateName ("NSTableViewRowPredicate")] [DefaultValue (true)]
		bool ShouldSelectRow (NSTableView tableView, nint row);
	
		[Export ("tableView:selectionIndexesForProposedSelection:"), DelegateName ("NSTableViewIndexFilter"), DefaultValueFromArgument ("proposedSelectionIndexes")]
		NSIndexSet GetSelectionIndexes (NSTableView tableView, NSIndexSet proposedSelectionIndexes);
	
		[Export ("tableView:shouldSelectTableColumn:"), DelegateName ("NSTableViewColumnPredicate"), DefaultValue (true)]
		bool ShouldSelectTableColumn (NSTableView tableView, NSTableColumn tableColumn);
	
		[Export ("tableView:mouseDownInHeaderOfTableColumn:"), EventArgs ("NSTableViewTable")]
		void MouseDownInHeaderOfTableColumn (NSTableView tableView, NSTableColumn tableColumn);
	
		[Export ("tableView:didClickTableColumn:"), EventArgs ("NSTableViewTable")]
		void DidClickTableColumn (NSTableView tableView, NSTableColumn tableColumn);
	
		[Export ("tableView:didDragTableColumn:"), EventArgs ("NSTableViewTable")]
		void DidDragTableColumn (NSTableView tableView, NSTableColumn tableColumn);
	
		[Export ("tableView:heightOfRow:"), DelegateName ("NSTableViewRowHeight"), NoDefaultValue]
		nfloat GetRowHeight (NSTableView tableView, nint row );
	
		[Export ("tableView:typeSelectStringForTableColumn:row:"), DelegateName ("NSTableViewColumnRowString"), DefaultValue ("String.Empty")]
		string GetSelectString (NSTableView tableView, NSTableColumn tableColumn, nint row );
	
		[Export ("tableView:nextTypeSelectMatchFromRow:toRow:forString:"), DelegateName ("NSTableViewSearchString"), DefaultValue (-1)]
		nint GetNextTypeSelectMatch (NSTableView tableView, nint startRow, nint endRow, string searchString);
	
		[Export ("tableView:shouldTypeSelectForEvent:withCurrentSearchString:"), DelegateName ("NSTableViewEventString"), DefaultValue (false)]
		bool ShouldTypeSelect (NSTableView tableView, NSEvent theEvent, string searchString );
	
		[Export ("tableView:shouldShowCellExpansionForTableColumn:row:"), DelegateName ("NSTableViewColumnRowPredicate"), DefaultValue (false)]
		bool ShouldShowCellExpansion (NSTableView tableView, NSTableColumn tableColumn, nint row );
	
		[Export ("tableView:shouldTrackCell:forTableColumn:row:"), DelegateName ("NSTableViewCell"), DefaultValue (false)]
		bool ShouldTrackCell (NSTableView tableView, NSCell cell, NSTableColumn tableColumn, nint row );
	
		[Export ("tableView:dataCellForTableColumn:row:"), DelegateName ("NSTableViewCellGetter"), NoDefaultValue]
		NSCell GetDataCell (NSTableView tableView, NSTableColumn tableColumn, nint row );
	
		[Export ("tableView:isGroupRow:"), DelegateName ("NSTableViewRowPredicate"), DefaultValue (false)]
		bool IsGroupRow (NSTableView tableView, nint row );
	
		[Export ("tableView:sizeToFitWidthOfColumn:"), DelegateName ("NSTableViewColumnWidth"), DefaultValue (80)]
		nfloat GetSizeToFitColumnWidth (NSTableView tableView, nint column );
	
		[Export ("tableView:shouldReorderColumn:toColumn:"), DelegateName ("NSTableReorder"), DefaultValue (false)]
		bool ShouldReorder (NSTableView tableView, nint columnIndex, nint newColumnIndex );
	
		[Export ("tableViewSelectionDidChange:"), EventArgs ("NSNotification")]
		void SelectionDidChange (NSNotification notification);
	
		[Export ("tableViewColumnDidMove:"), EventArgs ("NSNotification")]
		void ColumnDidMove (NSNotification notification);
	
		[Export ("tableViewColumnDidResize:"), EventArgs ("NSNotification")]
		void ColumnDidResize (NSNotification notification);
	
		[Export ("tableViewSelectionIsChanging:"), EventArgs ("NSNotification")]
		void SelectionIsChanging (NSNotification notification);

		[Lion]
                [Export ("tableView:viewForTableColumn:row:"), DelegateName ("NSTableViewViewGetter"), NoDefaultValue]
                NSView GetViewForItem (NSTableView tableView, NSTableColumn tableColumn, nint row);

		[Lion]
                [Export ("tableView:rowViewForRow:"), DelegateName ("NSTableViewRowGetter"), DefaultValue (null)]
                NSTableRowView CoreGetRowView (NSTableView tableView, nint row);

		[Lion]
                [Export ("tableView:didAddRowView:forRow:"), EventArgs ("NSTableViewRow")]
                void DidAddRowView (NSTableView tableView, NSTableRowView rowView, nint row);

		[Lion]
                [Export ("tableView:didRemoveRowView:forRow:"), EventArgs ("NSTableViewRow")]
                void DidRemoveRowView (NSTableView tableView, NSTableRowView rowView, nint row);

	}

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSTableViewDataSource {
		[Export ("numberOfRowsInTableView:")]
		nint GetRowCount (NSTableView tableView);
	
		[Export ("tableView:objectValueForTableColumn:row:")]
		NSObject GetObjectValue (NSTableView tableView, NSTableColumn tableColumn, nint row);
	
		[Export ("tableView:setObjectValue:forTableColumn:row:")]
		void SetObjectValue (NSTableView tableView, NSObject theObject, NSTableColumn tableColumn, nint row);
	
		[Export ("tableView:sortDescriptorsDidChange:")]
		void SortDescriptorsChanged (NSTableView tableView, NSSortDescriptor [] oldDescriptors);
	
		[Export ("tableView:writeRowsWithIndexes:toPasteboard:")]
		bool WriteRows (NSTableView tableView, NSIndexSet rowIndexes, NSPasteboard pboard );
	
		[Export ("tableView:validateDrop:proposedRow:proposedDropOperation:")]
		NSDragOperation ValidateDrop (NSTableView tableView, NSDraggingInfo info, nint row, NSTableViewDropOperation dropOperation);
	
		[Export ("tableView:acceptDrop:row:dropOperation:")]
		bool AcceptDrop (NSTableView tableView, NSDraggingInfo info, nint row, NSTableViewDropOperation dropOperation);
	
		[Export ("tableView:namesOfPromisedFilesDroppedAtDestination:forDraggedRowsWithIndexes:")]
		string [] FilesDropped (NSTableView tableView, NSUrl dropDestination, NSIndexSet indexSet );

		[Lion]
                [Export ("tableView:pasteboardWriterForRow:")]
                NSPasteboardWriting GetPasteboardWriterForRow (NSTableView tableView, nint row);

		[Lion]
                [Export ("tableView:draggingSession:willBeginAtPoint:forRowIndexes:")]
                void DraggingSessionWillBegin (NSTableView tableView, NSDraggingSession draggingSession, CGPoint willBeginAtScreenPoint, NSIndexSet rowIndexes);

		[Lion]
                [Export ("tableView:draggingSession:endedAtPoint:operation:")]
                void DraggingSessionEnded (NSTableView tableView, NSDraggingSession draggingSession, CGPoint endedAtScreenPoint, NSDragOperation operation);

		[Lion]
                [Export ("tableView:updateDraggingItemsForDrag:")]
                void UpdateDraggingItems (NSTableView tableView, NSDraggingInfo draggingInfo);
	}

	//64 bit reviewed
	//
	// This is the mixed NSTableViewDataSource and NSTableViewDelegate
	//
	[Model]
	[BaseType (typeof (NSObject))]
	public interface NSTableViewSource {
		//
		// These come form NSTableViewDataSource
		//
		[Export ("tableView:willDisplayCell:forTableColumn:row:")]
		void WillDisplayCell (NSTableView tableView, NSObject cell, NSTableColumn tableColumn, nint row);
	
		[Export ("tableView:shouldEditTableColumn:row:")] [DefaultValue (false)]
		bool ShouldEditTableColumn (NSTableView tableView, NSTableColumn tableColumn, nint row);
	
		[Export ("selectionShouldChangeInTableView:")] [DefaultValue (false)]
		bool SelectionShouldChange (NSTableView tableView);
	
		[Export ("tableView:shouldSelectRow:")] [DefaultValue (true)]
		bool ShouldSelectRow (NSTableView tableView, nint row);
	
		[Export ("tableView:selectionIndexesForProposedSelection:")]
		NSIndexSet GetSelectionIndexes (NSTableView tableView, NSIndexSet proposedSelectionIndexes);
	
		[Export ("tableView:shouldSelectTableColumn:")] [DefaultValue (true)]
		bool ShouldSelectTableColumn (NSTableView tableView, NSTableColumn tableColumn);
	
		[Export ("tableView:mouseDownInHeaderOfTableColumn:")]
		void MouseDown (NSTableView tableView, NSTableColumn tableColumn);
	
		[Export ("tableView:didClickTableColumn:")]
		void DidClickTableColumn (NSTableView tableView, NSTableColumn tableColumn);
	
		[Export ("tableView:didDragTableColumn:")]
		void DidDragTableColumn (NSTableView tableView, NSTableColumn tableColumn);
	
		//FIXME: Binding CGRectPointer
		//[Export ("tableView:toolTipForCell:rect:tableColumn:row:mouseLocation:")]
		//string TableViewtoolTipForCellrecttableColumnrowmouseLocation (NSTableView tableView, NSCell cell, CGRectPointer rect, NSTableColumn tableColumn, int row, CGPoint mouseLocation);
	
		[Export ("tableView:heightOfRow:")]
		nfloat GetRowHeight (NSTableView tableView, nint row );
	
		[Export ("tableView:typeSelectStringForTableColumn:row:")]
		string GetSelectString (NSTableView tableView, NSTableColumn tableColumn, nint row );
	
		[Export ("tableView:nextTypeSelectMatchFromRow:toRow:forString:")]
		nint GetNextTypeSelectMatch (NSTableView tableView, nint startRow, nint endRow, string searchString );
	
		[Export ("tableView:shouldTypeSelectForEvent:withCurrentSearchString:")]
		bool ShouldTypeSelect (NSTableView tableView, NSEvent theEvent, string searchString );
	
		[Export ("tableView:shouldShowCellExpansionForTableColumn:row:")]
		bool ShouldShowCellExpansion (NSTableView tableView, NSTableColumn tableColumn, nint row );
	
		[Export ("tableView:shouldTrackCell:forTableColumn:row:")]
		bool ShouldTrackCell (NSTableView tableView, NSCell cell, NSTableColumn tableColumn, nint row );
	
		[Export ("tableView:dataCellForTableColumn:row:")]
		NSCell GetCell (NSTableView tableView, NSTableColumn tableColumn, nint row );
	
		[Export ("tableView:isGroupRow:"), DefaultValue (false)]
		bool IsGroupRow (NSTableView tableView, nint row );
	
		[Export ("tableView:sizeToFitWidthOfColumn:")]
		nfloat GetSizeToFitColumnWidth (NSTableView tableView, nint column );
	
		[Export ("tableView:shouldReorderColumn:toColumn:")]
		bool ShouldReorder (NSTableView tableView, nint columnIndex, nint newColumnIndex );
	
		[Export ("tableViewSelectionDidChange:")]
		void SelectionDidChange (NSNotification notification);
	
		[Export ("tableViewColumnDidMove:")]
		void ColumnDidMove (NSNotification notification);
	
		[Export ("tableViewColumnDidResize:")]
		void ColumnDidResize (NSNotification notification);
	
		[Export ("tableViewSelectionIsChanging:")]
		void SelectionIsChanging (NSNotification notification);

		// NSTableViewDataSource
		[Export ("numberOfRowsInTableView:")]
		nint GetRowCount (NSTableView tableView);
	
		[Export ("tableView:objectValueForTableColumn:row:")]
		NSObject GetObjectValue (NSTableView tableView, NSTableColumn tableColumn, nint row);
	
		[Export ("tableView:setObjectValue:forTableColumn:row:")]
		void SetObjectValue (NSTableView tableView, NSObject theObject, NSTableColumn tableColumn, nint row);
	
		[Export ("tableView:sortDescriptorsDidChange:")]
		void SortDescriptorsChanged (NSTableView tableView, NSSortDescriptor [] oldDescriptors);
	
		[Export ("tableView:writeRowsWithIndexes:toPasteboard:")]
		bool WriteRows (NSTableView tableView, NSIndexSet rowIndexes, NSPasteboard pboard );
	
		[Export ("tableView:validateDrop:proposedRow:proposedDropOperation:")]
		NSDragOperation ValidateDrop (NSTableView tableView, NSDraggingInfo info, nint row, NSTableViewDropOperation dropOperation);
	
		[Export ("tableView:acceptDrop:row:dropOperation:")]
		bool AcceptDrop (NSTableView tableView, NSDraggingInfo info, nint row, NSTableViewDropOperation dropOperation);
	
		[Export ("tableView:namesOfPromisedFilesDroppedAtDestination:forDraggedRowsWithIndexes:")]
		string [] FilesDropped (NSTableView tableView, NSUrl dropDestination, NSIndexSet indexSet );
		
		[Lion]
                [Export ("tableView:viewForTableColumn:row:")]
                NSView GetViewForItem (NSTableView tableView, NSTableColumn tableColumn, nint row);

		[Lion]
                [Export ("tableView:rowViewForRow:")]
                NSTableRowView GetRowView (NSTableView tableView, nint row);

		[Lion]
                [Export ("tableView:didAddRowView:forRow:")]
                void DidAddRowView (NSTableView tableView, NSTableRowView rowView, nint row);

		[Lion]
                [Export ("tableView:didRemoveRowView:forRow:")]
                void DidRemoveRowView (NSTableView tableView, NSTableRowView rowView, nint row);

		[Lion]
                [Export ("tableView:pasteboardWriterForRow:")]
                NSPasteboardWriting GetPasteboardWriterForRow (NSTableView tableView, nint row);

		[Lion]
                [Export ("tableView:draggingSession:willBeginAtPoint:forRowIndexes:")]
                void DraggingSessionWillBegin (NSTableView tableView, NSDraggingSession draggingSession, CGPoint willBeginAtScreenPoint, NSIndexSet rowIndexes);

		[Lion]
                [Export ("tableView:draggingSession:endedAtPoint:operation:")]
                void DraggingSessionEnded (NSTableView tableView, NSDraggingSession draggingSession, CGPoint endedAtScreenPoint, NSDragOperation operation);

		[Lion]
                [Export ("tableView:updateDraggingItemsForDrag:")]
                void UpdateDraggingItems (NSTableView tableView, NSDraggingInfo draggingInfo);
	}

	//64 bit reviewed
	[BaseType (typeof (NSTextFieldCell))]
	public interface NSTableHeaderCell {
		[Export ("drawSortIndicatorWithFrame:inView:ascending:priority:")]
		void DrawSortIndicator (CGRect cellFrame, NSView controlView, bool ascending, nint priority );
	
		[Export ("sortIndicatorRectForBounds:")]
		CGRect GetSortIndicatorRect (CGRect theRect );
	}

	//64 bit reviewed
	[BaseType (typeof (NSView))]
	public interface NSTableHeaderView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("draggedColumn")]
		nint DraggedColumn { get; }
	
		[Export ("draggedDistance")]
		nfloat DraggedDistance { get; }
	
		[Export ("resizedColumn")]
		nint ResizedColumn { get; }
	
		[Export ("headerRectOfColumn:")]
		CGRect GetHeaderRect (nint column);
	
		[Export ("columnAtPoint:")]
		nint GetColumn (CGPoint point);
	
		//Detected properties
		[Export ("tableView")]
		NSTableView TableView { get; set; }
	}
		
	//64 bit reviewed
	[BaseType (typeof (NSView), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTabViewDelegate)})]
	public partial interface NSTabView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("selectTabViewItem:")]
		void Select (NSTabViewItem tabViewItem);

		[Export ("selectTabViewItemAtIndex:")]
		void SelectAt (nint index);

		[Export ("selectTabViewItemWithIdentifier:")]
		void Select (NSObject identifier);

		[Export ("takeSelectedTabViewItemFromSender:")]
		void TakeSelectedTabViewItemFrom (NSObject sender);

		[Export ("selectFirstTabViewItem:")]
		void SelectFirst (NSObject sender);

		[Export ("selectLastTabViewItem:")]
		void SelectLast (NSObject sender);

		[Export ("selectNextTabViewItem:")]
		void SelectNext (NSObject sender);

		[Export ("selectPreviousTabViewItem:")]
		void SelectPrevious (NSObject sender);

		[Export ("selectedTabViewItem")]
		NSTabViewItem Selected { get; }

		[Export ("font")]
		NSFont Font { get; set; }

		[Export ("tabViewType")]
		NSTabViewType TabViewType { get; set; }

		[Export ("tabViewItems")]
		NSTabViewItem [] Items { get; }

		[Export ("allowsTruncatedLabels")]
		bool AllowsTruncatedLabels { get; set; }

		[Export ("minimumSize")]
		CGSize MinimumSize { get; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("controlTint")]
		NSControlTint ControlTint { get; set; }

		[Export ("controlSize")]
		NSControlSize ControlSize { get; set; }

		[Export ("addTabViewItem:")][PostGet ("Items")]
		void Add (NSTabViewItem tabViewItem);

		[Export ("insertTabViewItem:atIndex:")][PostGet ("Items")]
		void Insert (NSTabViewItem tabViewItem, nint index);

		[Export ("removeTabViewItem:")][PostGet ("Items")]
		void Remove (NSTabViewItem tabViewItem);

		[Export ("delegate"), NullAllowed]
		NSTabViewDelegate Delegate { get; set; }

		[Export ("tabViewItemAtPoint:")]
		NSTabViewItem TabViewItemAtPoint (CGPoint point);

		[Export ("contentRect")]
		CGRect ContentRect { get; }

		[Export ("numberOfTabViewItems")]
		nint Count { get; }

		[Export ("indexOfTabViewItem:")]
		nint IndexOf (NSTabViewItem tabViewItem);

		[Export ("tabViewItemAtIndex:")]
		NSTabViewItem Item (nint index);

		[Export ("indexOfTabViewItemWithIdentifier:")]
		nint IndexOf (NSObject identifier);
	}

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSTabViewDelegate {
		[Export ("tabView:shouldSelectTabViewItem:"), DelegateName ("NSTabViewPredicate"), DefaultValue (true)]
		bool ShouldSelectTabViewItem (NSTabView tabView, NSTabViewItem item);
		
		[Export ("tabView:willSelectTabViewItem:"), EventArgs ("NSTabViewItem")]
		void WillSelect (NSTabView tabView, NSTabViewItem item);

		[Export ("tabView:didSelectTabViewItem:"), EventArgs ("NSTabViewItem")]
		void DidSelect (NSTabView tabView, NSTabViewItem item);
	 
		[Export ("tabViewDidChangeNumberOfTabViewItems:")]
		void NumberOfItemsChanged (NSTabView tabView);
	}

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	public interface NSTabViewItem {
		[Export ("initWithIdentifier:")]
		IntPtr Constructor (NSObject identifier);

		[Export ("identifier")]
		NSObject Identifier { get; set; }

		[Export ("view")]
		NSView View { get; set; }

		[Export ("initialFirstResponder")]
		NSObject InitialFirstResponder { get; set; }

		[Export ("label")]
		string Label { get; set; }

		[Export ("color")]
		NSColor Color { get; set; }

		[Export ("tabState")]
		NSTabState TabState { get; }

		[Export ("tabView")]
		NSTabView TabView { get; }

		[Export ("drawLabel:inRect:")]
		void DrawLabel (bool shouldTruncateLabel, CGRect labelRect);

		[Export ("sizeOfLabel:")]
		CGSize SizeOfLabel (bool computeMin);
	}

	//64 bit reviewed
	[BaseType (typeof (NSView), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTextDelegate)})]
	public partial interface NSText {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("replaceCharactersInRange:withString:")]
		void Replace (NSRange range, string aString);

		[Export ("replaceCharactersInRange:withRTF:")]
		void ReplaceWithRtf (NSRange range, NSData rtfData);

		[Export ("replaceCharactersInRange:withRTFD:")]
		void ReplaceWithRtfd (NSRange range, NSData rtfdData);

		[Export ("RTFFromRange:")]
		NSData RtfFromRange (NSRange range);

		[Export ("RTFDFromRange:")]
		NSData RtfdFromRange (NSRange range);

		[Export ("writeRTFDToFile:atomically:")]
		bool WriteRtfd (string path, bool atomically);

		[Export ("readRTFDFromFile:")]
		bool FromRtfdFile (string path);

		[Export ("isRulerVisible")]
		bool IsRulerVisible { get; }

		[Export ("scrollRangeToVisible:")]
		void ScrollRangeToVisible (NSRange range);

		[Export ("setTextColor:range:")]
		void SetTextColor (NSColor color, NSRange range);

		[Export ("setFont:range:")]
		void SetFont (NSFont font, NSRange range);

		[Export ("sizeToFit")]
		void SizeToFit ();

		[Export ("copy:")]
		void Copy (NSObject sender);

		[Export ("copyFont:")]
		void CopyFont (NSObject sender);

		[Export ("copyRuler:")]
		void CopyRuler (NSObject sender);

		[Export ("cut:")]
		void Cut (NSObject sender);

		[Export ("delete:")]
		void Delete (NSObject sender);

		[Export ("paste:")]
		void Paste (NSObject sender);

		[Export ("pasteFont:")]
		void PasteFont (NSObject sender);

		[Export ("pasteRuler:")]
		void PasteRuler (NSObject sender);

		[Export ("selectAll:")]
		void SelectAll (NSObject sender);

		[Export ("changeFont:")]
		void ChangeFont (NSObject sender);

		[Export ("alignLeft:")]
		void AlignLeft (NSObject sender);

		[Export ("alignRight:")]
		void AlignRight (NSObject sender);

		[Export ("alignCenter:")]
		void AlignCenter (NSObject sender);

		[Export ("subscript:")]
		void Subscript (NSObject sender);

		[Export ("superscript:")]
		void Superscript (NSObject sender);

		[Export ("underline:")]
		void Underline (NSObject sender);

		[Export ("unscript:")]
		void Unscript (NSObject sender);

		[Export ("showGuessPanel:")]
		void ShowGuessPanel (NSObject sender);

		[Export ("checkSpelling:")]
		void CheckSpelling (NSObject sender);

		[Export ("toggleRuler:")]
		void ToggleRuler (NSObject sender);

		//Detected properties
		[Export ("string")]
		string Value { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSTextDelegate Delegate { get; set; }
		
		[Export ("editable")]
		bool Editable { [Bind ("isEditable")]get; set; }

		[Export ("selectable")]
		bool Selectable { [Bind ("isSelectable")]get; set; }

		[Export ("richText")]
		bool RichText { [Bind ("isRichText")]get; set; }

		[Export ("importsGraphics")]
		bool ImportsGraphics { get; set; }

		[Export ("fieldEditor")]
		bool FieldEditor { [Bind ("isFieldEditor")]get; set; }

		[Export ("usesFontPanel")]
		bool UsesFontPanel { get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("selectedRange")]
		NSRange SelectedRange { get; set; }

		[Export ("font")]
		NSFont Font { get; set; }

		[Export ("textColor")]
		NSColor TextColor { get; set; }

		[Export ("alignment")]
		NSTextAlignment Alignment { get; set; }

		[Export ("baseWritingDirection")]
		NSWritingDirection BaseWritingDirection { get; set; }

		[Export ("maxSize")]
		CGSize MaxSize { get; set; }

		[Export ("minSize")]
		CGSize MinSize { get; set; }

		[Export ("horizontallyResizable")]
		bool HorizontallyResizable { [Bind ("isHorizontallyResizable")]get; set; }

		[Export ("verticallyResizable")]
		bool VerticallyResizable { [Bind ("isVerticallyResizable")]get; set; }
	}

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSTextDelegate {
		[Export ("textShouldBeginEditing:"), DelegateName ("NSTextPredicate"), DefaultValue (true)]
		bool TextShouldBeginEditing (NSText textObject);

		[Export ("textShouldEndEditing:"), DelegateName ("NSTextPredicate"), DefaultValue (true)]
		bool TextShouldEndEditing (NSText textObject);

		[Export ("textDidBeginEditing:"), EventArgs ("NSNotification")]
		void TextDidBeginEditing (NSNotification notification);

		[Export ("textDidEndEditing:"), EventArgs ("NSNotification")]
		void TextDidEndEditing (NSNotification notification);

		[Export ("textDidChange:"), EventArgs ("NSNotification")]
		void TextDidChange (NSNotification notification);
	}

	//64 bit reviewed
	[BaseType (typeof (NSCell))]
	public interface NSTextAttachmentCell {
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("wantsToTrackMouse")]
		bool WantsToTrackMouse ();

		[Export ("highlight:withFrame:inView:")]
		void Highlight (bool highlight, CGRect cellFrame, NSView controlView);

		[Export ("trackMouse:inRect:ofView:untilMouseUp:")]
		bool TrackMouse (NSEvent theEvent, CGRect cellFrame, NSView controlView, bool untilMouseUp);

		[Export ("cellSize")]
		CGSize CellSize { get; }

		[Export ("cellBaselineOffset")]
		CGPoint CellBaselineOffset { get; }

		[Export ("drawWithFrame:inView:characterIndex:")]
		void DrawWithFrame (CGRect cellFrame, NSView controlView, nuint charIndex);

		[Export ("drawWithFrame:inView:characterIndex:layoutManager:")]
		void DrawWithFrame (CGRect cellFrame, NSView controlView, nuint charIndex, NSLayoutManager layoutManager);

		[Export ("wantsToTrackMouseForEvent:inRect:ofView:atCharacterIndex:")]
		bool WantsToTrackMouse (NSEvent theEvent, CGRect cellFrame, NSView controlView, nuint charIndex);

		[Export ("trackMouse:inRect:ofView:atCharacterIndex:untilMouseUp:")]
		bool TrackMouse (NSEvent theEvent, CGRect cellFrame, NSView controlView, nuint charIndex, bool untilMouseUp);

		[Export ("cellFrameForTextContainer:proposedLineFragment:glyphPosition:characterIndex:")]
		CGRect CellFrameForTextContainer (NSTextContainer textContainer, CGRect lineFrag, CGPoint position, nuint charIndex);

		//Detected properties
		[Export ("attachment")]
		NSTextAttachment Attachment { get; set; }
	}

	// 64bit reviewed
	[BaseType (typeof (NSObject))]
	public interface NSTextAttachment {
		[Export ("initWithFileWrapper:")]
		IntPtr Constructor (NSFileWrapper fileWrapper);

		//Detected properties
		[Export ("fileWrapper")]
		NSFileWrapper FileWrapper { get; set; }

		[Export ("attachmentCell")]
		NSTextAttachmentCell AttachmentCell { get; set; }

	}

	// 64 bit reviewed
	[BaseType (typeof (NSObject))]
	public interface NSTextBlock {
		[Export ("setValue:type:forDimension:")]
		void SetValue (nfloat val, NSTextBlockValueType type, NSTextBlockDimension dimension);

		[Export ("valueForDimension:")]
		nfloat GetValue (NSTextBlockDimension dimension);

		[Export ("valueTypeForDimension:")]
		NSTextBlockValueType GetValueType (NSTextBlockDimension dimension);

		[Export ("setContentWidth:type:")]
		void SetContentWidth (nfloat val, NSTextBlockValueType type);

		[Export ("contentWidth")]
		nfloat ContentWidth { get; }

		[Export ("contentWidthValueType")]
		NSTextBlockValueType ContentWidthValueType { get; }

		[Export ("setWidth:type:forLayer:edge:")]
		void SetWidth (nfloat val, NSTextBlockValueType type, NSTextBlockLayer layer, NSRectEdge edge);

		[Export ("setWidth:type:forLayer:")]
		void SetWidth (nfloat val, NSTextBlockValueType type, NSTextBlockLayer layer);

		[Export ("widthForLayer:edge:")]
		nfloat GetWidth (NSTextBlockLayer layer, NSRectEdge edge);

		[Export ("widthValueTypeForLayer:edge:")]
		NSTextBlockValueType WidthValueTypeForLayer (NSTextBlockLayer layer, NSRectEdge edge);

		[Export ("setBorderColor:forEdge:")]
		void SetBorderColor (NSColor color, NSRectEdge edge);

		[Export ("setBorderColor:")]
		void SetBorderColor (NSColor color);

		[Export ("borderColorForEdge:")]
		NSColor GetBorderColor (NSRectEdge edge);

		[Export ("rectForLayoutAtPoint:inRect:textContainer:characterRange:")]
		CGRect GetRectForLayout (CGPoint startingPoint, CGRect rect, NSTextContainer textContainer, NSRange charRange);

		[Export ("boundsRectForContentRect:inRect:textContainer:characterRange:")]
		CGRect GetBoundsRect (CGRect contentRect, CGRect rect, NSTextContainer textContainer, NSRange charRange);

		[Export ("drawBackgroundWithFrame:inView:characterRange:layoutManager:")]
		void DrawBackground (CGRect frameRect, NSView controlView, NSRange charRange, NSLayoutManager layoutManager);

		//Detected properties
		[Export ("verticalAlignment")]
		NSTextBlockVerticalAlignment VerticalAlignment { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

	}
	//64 bit reviewed
	[BaseType (typeof (NSControl), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTextFieldDelegate)})]
	public partial interface NSTextField {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);
		
		[Export ("selectText:")]
		void SelectText (NSObject sender);

		[Export ("textShouldBeginEditing:")]
		bool ShouldBeginEditing (NSText textObject);

		[Export ("textShouldEndEditing:")]
		bool ShouldEndEditing (NSText textObject);

		[Export ("textDidBeginEditing:")]
		void DidBeginEditing (NSNotification notification);

		[Export ("textDidEndEditing:")]
		void DidEndEditing (NSNotification notification);

		[Export ("textDidChange:")]
		void DidChange (NSNotification notification);

		[Export ("acceptsFirstResponder")]
		bool AcceptsFirstResponder ();

		//Detected properties
		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("textColor")]
		NSColor TextColor { get; set; }

		[Export ("bordered")]
		bool Bordered { [Bind ("isBordered")]get; set; }

		[Export ("bezeled")]
		bool Bezeled { [Bind ("isBezeled")]get; set; }

		[Export ("editable")]
		bool Editable { [Bind ("isEditable")]get; set; }

		[Export ("selectable")]
		bool Selectable { [Bind ("isSelectable")]get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Wrap ("WeakDelegate")]
		NSTextFieldDelegate Delegate { get; set; }

		[Export ("bezelStyle")]
		NSTextFieldBezelStyle BezelStyle { get; set; }

		[Export ("allowsEditingTextAttributes")]
		bool AllowsEditingTextAttributes { get; set; }

		[Export ("importsGraphics")]
		bool ImportsGraphics { get; set; }

		[MountainLion, Export ("preferredMaxLayoutWidth")]
		nfloat PreferredMaxLayoutWidth { get; set; }
	}
	// 64 bit reviewed
	[BaseType (typeof (NSTextField))]
	public interface NSSecureTextField {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSTextFieldDelegate {
		[Export ("control:textShouldBeginEditing:"), DelegateName ("NSControlText"), DefaultValue (true)]
		bool TextShouldBeginEditing (NSControl control, NSText fieldEditor);

		[Export ("control:textShouldEndEditing:"), DelegateName ("NSControlText"), DefaultValue (true)]
		bool TextShouldEndEditing (NSControl control, NSText fieldEditor);

		[Export ("control:didFailToFormatString:errorDescription:"), DelegateName ("NSControlTextError"), DefaultValue (true)]
		bool DidFailToFormatString (NSControl control, string str, string error);
		
		[Export ("control:didFailToValidatePartialString:errorDescription:"), EventArgs ("NSControlTextError")]
		void DidFailToValidatePartialString (NSControl control, string str, string error);
		
		[Export ("control:isValidObject:"), DelegateName ("NSControlTextValidation"), DefaultValue (true)]
		bool IsValidObject (NSControl control, NSObject objectToValidate);

		[Export ("control:textView:doCommandBySelector:"), DelegateName ("NSControlCommand"), DefaultValue (false)]
		bool DoCommandBySelector (NSControl control, NSTextView textView, Selector commandSelector);

		[Export ("control:textView:completions:forPartialWordRange:indexOfSelectedItem:"), DelegateName ("NSControlTextFilter"), DefaultValue ("new string[0]")]
		string [] GetCompletions (NSControl control, NSTextView textView, string [] words, NSRange charRange, nint index);

		[Export ("controlTextDidEndEditing:"), EventArgs ("NSNotification")]
		void EditingEnded (NSNotification notification);

		[Export ("controlTextDidChange:"), EventArgs ("NSNotification")]
		void Changed (NSNotification notification);

		[Export ("controlTextDidBeginEditing:"), EventArgs ("NSNotification")]
		void EditingBegan (NSNotification notification);	
	}

	//64 bit reviewed
	[BaseType (typeof (NSActionCell))]
	public interface NSTextFieldCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("setUpFieldEditorAttributes:")]
		NSText SetUpFieldEditorAttributes (NSText textObj);
	
		//Detected properties
		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }
	
		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }
	
		[Export ("textColor")]
		NSColor TextColor { get; set; }
	
		[Export ("bezelStyle")]
		NSTextFieldBezelStyle BezelStyle { get; set; }
	
		[Export ("placeholderString")]
		string PlaceholderString { get; set; }
	
		[Export ("placeholderAttributedString")]
		NSAttributedString PlaceholderAttributedString { get; set; }
	
		[Export ("allowedInputSourceLocales")]
		string [] AllowedInputSourceLocales { get; set; }

		[Export ("wantsNotificationForMarkedText")]
		[Override]
		bool WantsNotificationForMarkedText { get; set; }
	}

	[BaseType (typeof (NSTextFieldCell))]
	public interface NSSecureTextFieldCell {
		[Export ("echosBullets")]
		bool EchosBullets { get; set; }
	}  

	// 64 bit reviewed
	[BaseType (typeof (NSObject))]
	public partial interface NSTextInputContext {
		[Static]
		[Export ("currentInputContext")]
		NSTextInputContext CurrentInputContext { get; }

		[Export ("activate")]
		void Activate ();

		[Export ("deactivate")]
		void Deactivate ();

		[Export ("handleEvent:")]
		bool HandleEvent (NSEvent theEvent);

		[Export ("discardMarkedText")]
		void DiscardMarkedText ();

		[Export ("invalidateCharacterCoordinates")]
		void InvalidateCharacterCoordinates ();

		[Static]
		[Export ("localizedNameForInputSource:")]
		string LocalizedNameForInputSource (string inputSourceIdentifier);
	}

	// 64 bit reviewed
	[BaseType (typeof (NSObject))]
	public interface NSTextList {
		[Export ("initWithMarkerFormat:options:")]
		IntPtr Constructor (string format, NSTextListOptions mask);

		[Export ("markerFormat")]
		string MarkerFormat { get; }

		[Export ("listOptions")]
		NSTextListOptions ListOptions { get; }

		[Export ("markerForItemNumber:")]
		string GetMarker (nint itemNum);

		//Detected properties
		[Export ("startingItemNumber")]
		nint StartingItemNumber { get; set; }

	}

	//64 bit reviewed
	[BaseType (typeof (NSTextBlock))]
	public interface NSTextTableBlock {
		[Export ("initWithTable:startingRow:rowSpan:startingColumn:columnSpan:")]
		IntPtr Constructor (NSTextTable table, nint row, nint rowSpan, nint col, nint colSpan);

		[Export ("table")]
		NSTextTable Table { get; }

		[Export ("startingRow")]
		nint StartingRow { get; }

		[Export ("rowSpan")]
		nint RowSpan { get; }

		[Export ("startingColumn")]
		nint StartingColumn { get; }

		[Export ("columnSpan")]
		nint ColumnSpan { get; }
	}

	// 64 bit reviewed
	[BaseType (typeof (NSTextBlock))]
	public interface NSTextTable {
		[Export ("rectForBlock:layoutAtPoint:inRect:textContainer:characterRange:")]
		CGRect GetRectForBlock (NSTextTableBlock block, CGPoint startingPoint, CGRect rect, NSTextContainer textContainer, NSRange charRange);

		[Export ("boundsRectForBlock:contentRect:inRect:textContainer:characterRange:")]
		CGRect GetBoundsRect (NSTextTableBlock block, CGRect contentRect, CGRect rect, NSTextContainer textContainer, NSRange charRange);

		[Export ("drawBackgroundForBlock:withFrame:inView:characterRange:layoutManager:")]
		void DrawBackground (NSTextTableBlock block, CGRect frameRect, NSView controlView, NSRange charRange, NSLayoutManager layoutManager);

		//Detected properties
		[Export ("numberOfColumns")]
		nint Columns { get; set; }

		[Export ("layoutAlgorithm")]
		NSTextTableLayoutAlgorithm LayoutAlgorithm { get; set; }

		[Export ("collapsesBorders")]
		bool CollapsesBorders { get; set; }

		[Export ("hidesEmptyCells")]
		bool HidesEmptyCells { get; set; }
	}

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	public partial interface NSTextContainer {
        [Export("initWithContainerSize:"), Internal]
        [Sealed]
        IntPtr InitWithContainerSize(CGSize size);

        [Mac(10, 11)]
        [Export("initWithSize:"), Internal]
        [Sealed]
        IntPtr InitWithSize(CGSize size);

        [Export("replaceLayoutManager:")]
        void ReplaceLayoutManager(NSLayoutManager newLayoutManager);

        // FIXME: Binding
        //[Export ("lineFragmentRectForProposedRect:sweepDirection:movementDirection:remainingRect:")]
        //CGRect LineFragmentRect (CGRect proposedRect, NSLineSweepDirection sweepDirection, NSLineMovementDirection movementDirection, NSRectPointer remainingRect);

        [Export("isSimpleRectangularTextContainer")]
        bool IsSimpleRectangularTextContainer { get; }

        [Deprecated(PlatformName.MacOSX, 10, 11)]
        [Export("containsPoint:")]
        bool ContainsPoint(CGPoint point);

        //Detected properties
        [Export("layoutManager")]
        NSLayoutManager LayoutManager { get; set; }

        [Export("textView", ArgumentSemantic.Weak)]
        NSTextView TextView { get; set; }

        [Export("widthTracksTextView")]
        bool WidthTracksTextView { get; set; }

        [Export("heightTracksTextView")]
        bool HeightTracksTextView { get; set; }

        [Deprecated(PlatformName.MacOSX, 10, 11, message: "Use Size instead.")]
        [Export("containerSize")]
        CGSize ContainerSize { get; set; }

        [Export("lineFragmentPadding")]
        nfloat LineFragmentPadding { get; set; }

        [Mac(10, 11)]
        [Export("size", ArgumentSemantic.Assign)]
        CGSize Size { get; set; }

        [Mac(10, 11)]
        [Export("exclusionPaths", ArgumentSemantic.Copy)]
        // [Verify (StronglyTypedNSArray)]
        NSBezierPath[] ExclusionPaths { get; set; }

        [Mac(10, 11)]
        [Export("lineBreakMode", ArgumentSemantic.Assign)]
        NSLineBreakMode LineBreakMode { get; set; }

        [Mac(10, 11)]
        [Export("maximumNumberOfLines", ArgumentSemantic.Assign)]
        nuint MaximumNumberOfLines { get; set; }

        [Mac(10, 11)]
        [Export("lineFragmentRectForProposedRect:atIndex:writingDirection:remainingRect:")]
        CGRect GetLineFragmentRect(CGRect proposedRect, nuint characterIndex, NSWritingDirection baseWritingDirection, ref CGRect remainingRect);
    }
    //64 bit reviewed
    [BaseType (typeof (NSMutableAttributedString), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTextStorageDelegate)})]
	public partial interface NSTextStorage {
		[Export ("addLayoutManager:")][PostGet ("LayoutManagers")]
		void AddLayoutManager (NSLayoutManager obj);

		[Export ("removeLayoutManager:")][PostGet ("LayoutManagers")]
		void RemoveLayoutManager (NSLayoutManager obj);

		[Export ("layoutManagers")]
		NSLayoutManager [] LayoutManagers { get; }

		[Export ("edited:range:changeInLength:")]
		void Edited (nuint editedMask, NSRange range, nint delta);

		[Export ("processEditing")]
		void ProcessEditing ();

		[Export ("invalidateAttributesInRange:")]
		void InvalidateAttributes (NSRange range);

		[Export ("ensureAttributesAreFixedInRange:")]
		void EnsureAttributesAreFixed (NSRange range);

		[Export ("fixesAttributesLazily")]
		bool FixesAttributesLazily { get; }

		[Export ("editedMask")]
		NSTextStorageEditedFlags EditedMask { get; }

		[Export ("editedRange")]
		NSRange EditedRange { get; }

		[Export ("changeInLength")]
		nint ChangeInLength { get; }

		//Detected properties
		[Export ("delegate")]
		NSObject WeakDelegate { get; set; }
		[Wrap ("WeakDelegate")]
		NSTextStorageDelegate Delegate { get; set; }

	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSTextStorageDelegate {
		[Export ("textStorageWillProcessEditing:")]
		void TextStorageWillProcessEditing (NSNotification notification);

		[Export ("textStorageDidProcessEditing:")]
		void TextStorageDidProcessEditing (NSNotification notification);
	}

	// 64 bit reviewed
	[BaseType (typeof (NSObject))]
	public interface NSTextTab {
		[Export ("initWithTextAlignment:location:options:")]
		IntPtr Constructor (NSTextAlignment alignment, nfloat loc, NSDictionary options);

		[Export ("alignment")]
		NSTextAlignment Alignment { get; }

		[Export ("options")]
		NSDictionary Options { get; }

		[Export ("initWithType:location:")]
		IntPtr Constructor (NSTextTabType type, nfloat loc);

		[Export ("location")]
		nfloat Location { get; }

		[Export ("tabStopType")]
		NSTextTabType TabStopType { get; }
	}

	//64 bit reviewed
	[BaseType (typeof (NSText), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTextViewDelegate)})]
	public partial interface NSTextView : NSDraggingSource {
		[Export ("initWithFrame:textContainer:")]
		IntPtr Constructor (CGRect frameRect, NSTextContainer container);

		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("replaceTextContainer:")]
		void ReplaceTextContainer (NSTextContainer newContainer);

		[Export ("textContainerOrigin")]
		CGPoint TextContainerOrigin { get; }

		[Export ("invalidateTextContainerOrigin")]
		void InvalidateTextContainerOrigin ();

		[Export ("layoutManager")]
		NSLayoutManager LayoutManager { get; }

		[Export ("textStorage")]
		NSTextStorage TextStorage { get; }

		[Export ("insertText:")]
		void InsertText (NSObject insertString);

		[Export ("setConstrainedFrameSize:")]
		void SetConstrainedFrameSize (CGSize desiredSize);

		[Export ("setAlignment:range:")]
		void SetAlignmentRange (NSTextAlignment alignment, NSRange range);

		[Export ("setBaseWritingDirection:range:")]
		void SetBaseWritingDirection (NSWritingDirection writingDirection, NSRange range);

		[Export ("turnOffKerning:")]
		void TurnOffKerning (NSObject sender);

		[Export ("tightenKerning:")]
		void TightenKerning (NSObject sender);

		[Export ("loosenKerning:")]
		void LoosenKerning (NSObject sender);

		[Export ("useStandardKerning:")]
		void UseStandardKerning (NSObject sender);

		[Export ("turnOffLigatures:")]
		void TurnOffLigatures (NSObject sender);

		[Export ("useStandardLigatures:")]
		void UseStandardLigatures (NSObject sender);

		[Export ("useAllLigatures:")]
		void UseAllLigatures (NSObject sender);

		[Export ("raiseBaseline:")]
		void RaiseBaseline (NSObject sender);

		[Export ("lowerBaseline:")]
		void LowerBaseline (NSObject sender);

		[Export ("toggleTraditionalCharacterShape:")]
		void ToggleTraditionalCharacterShape (NSObject sender);

		[Export ("outline:")]
		void Outline (NSObject sender);

		[Export ("performFindPanelAction:")]
		void PerformFindPanelAction (NSObject sender);

		[Export ("alignJustified:")]
		void AlignJustified (NSObject sender);

		[Export ("changeColor:")]
		void ChangeColor (NSObject sender);

		[Export ("changeAttributes:")]
		void ChangeAttributes (NSObject sender);

		[Export ("changeDocumentBackgroundColor:")]
		void ChangeDocumentBackgroundColor (NSObject sender);

		[Export ("orderFrontSpacingPanel:")]
		void OrderFrontSpacingPanel (NSObject sender);

		[Export ("orderFrontLinkPanel:")]
		void OrderFrontLinkPanel (NSObject sender);

		[Export ("orderFrontListPanel:")]
		void OrderFrontListPanel (NSObject sender);

		[Export ("orderFrontTablePanel:")]
		void OrderFrontTablePanel (NSObject sender);

		[Export ("rulerView:didMoveMarker:")]
		void RulerViewDidMoveMarker (NSRulerView ruler, NSRulerMarker marker);

		[Export ("rulerView:didRemoveMarker:")]
		void RulerViewDidRemoveMarker (NSRulerView ruler, NSRulerMarker marker);

		[Export ("rulerView:didAddMarker:")]
		void RulerViewDidAddMarker (NSRulerView ruler, NSRulerMarker marker);

		[Export ("rulerView:shouldMoveMarker:")]
		bool RulerViewShouldMoveMarker (NSRulerView ruler, NSRulerMarker marker);

		[Export ("rulerView:shouldAddMarker:")]
		bool RulerViewShouldAddMarker (NSRulerView ruler, NSRulerMarker marker);

		[Export ("rulerView:willMoveMarker:toLocation:")]
		nfloat RulerViewWillMoveMarker (NSRulerView ruler, NSRulerMarker marker, nfloat location);

		[Export ("rulerView:shouldRemoveMarker:")]
		bool RulerViewShouldRemoveMarker (NSRulerView ruler, NSRulerMarker marker);

		[Export ("rulerView:willAddMarker:atLocation:")]
		nfloat RulerViewWillAddMarker (NSRulerView ruler, NSRulerMarker marker, nfloat location);

		[Export ("rulerView:handleMouseDown:")]
		void RulerViewHandleMouseDown (NSRulerView ruler, NSEvent theEvent);

		[Export ("setNeedsDisplayInRect:avoidAdditionalLayout:")]
		void SetNeedsDisplay (CGRect rect, bool avoidAdditionalLayout);

		[Export ("shouldDrawInsertionPoint")]
		bool ShouldDrawInsertionPoint { get; }

		[Export ("drawInsertionPointInRect:color:turnedOn:")]
		void DrawInsertionPoint (CGRect rect, NSColor color, bool turnedOn);

		[Export ("drawViewBackgroundInRect:")]
		void DrawViewBackgroundInRect (CGRect rect);

		[Export ("updateRuler")]
		void UpdateRuler ();

		[Export ("updateFontPanel")]
		void UpdateFontPanel ();

		[Export ("updateDragTypeRegistration")]
		void UpdateDragTypeRegistration ();

		[Export ("selectionRangeForProposedRange:granularity:")]
		NSRange SelectionRange (NSRange proposedCharRange, NSSelectionGranularity granularity);

		[Export ("clickedOnLink:atIndex:")]
		void ClickedOnLink (NSObject link, nuint charIndex);

		[Export ("startSpeaking:")]
		void StartSpeaking (NSObject sender);

		[Export ("stopSpeaking:")]
		void StopSpeaking (NSObject sender);

		[Export ("characterIndexForInsertionAtPoint:")]
		uint CharacterIndex (CGPoint point);

		//Detected properties
		[Export ("textContainer")]
		NSTextContainer TextContainer { get; set; }

		[Export ("textContainerInset")]
		CGSize TextContainerInset { get; set; }

		//
		// Completion support
		//
		[Export ("complete:")]
		void Complete ([NullAllowed] NSObject sender);

		[Export ("rangeForUserCompletion")]
		NSRange RangeForUserCompletion ();

		[Export ("completionsForPartialWordRange:indexOfSelectedItem:")]
		string [] CompletionsForPartialWord (NSRange charRange, out nint index);

		[Export ("insertCompletion:forPartialWordRange:movement:isFinal:")]
		void InsertCompletion (string completion, NSRange partialWordCharRange, nint movement, bool isFinal);

		// Pasteboard
		[Export ("writablePasteboardTypes")]
		string [] WritablePasteboardTypes ();

		[Export ("writeSelectionToPasteboard:type:")]
		bool WriteSelectionToPasteboard (NSPasteboard pboard, string type);

		[Export ("writeSelectionToPasteboard:types:")]
		bool WriteSelectionToPasteboard (NSPasteboard pboard, string [] types);

		[Export ("readablePasteboardTypes")]
		string [] ReadablePasteboardTypes ();

		[Export ("preferredPasteboardTypeFromArray:restrictedToTypesFromArray:")]
		string GetPreferredPasteboardType (string [] availableTypes, string [] allowedTypes);

		[Export ("readSelectionFromPasteboard:type:")]
		bool ReadSelectionFromPasteboard (NSPasteboard pboard, string type);

		[Export ("readSelectionFromPasteboard:")]
		bool ReadSelectionFromPasteboard (NSPasteboard pboard);

		[Static]
		[Export ("registerForServices")]
		void RegisterForServices ();

		[Export ("validRequestorForSendType:returnType:")]
		NSObject ValidRequestorForSendType (string sendType, string returnType);

		[Export ("pasteAsPlainText:")]
		void PasteAsPlainText (NSObject sender);

		[Export ("pasteAsRichText:")]
		void PasteAsRichText (NSObject sender);

		//
		// Dragging support
		//

		// FIXME: Binding
		//[Export ("dragImageForSelectionWithEvent:origin:")]
		//NSImage DragImageForSelection (NSEvent theEvent, CGPointPointer origin);

		[Export ("acceptableDragTypes")]
		string [] AcceptableDragTypes ();

		[Export ("dragOperationForDraggingInfo:type:")]
		NSDragOperation DragOperationForDraggingInfo (NSDraggingInfo dragInfo, string type);

		[Export ("cleanUpAfterDragOperation")]
		void CleanUpAfterDragOperation ();

		[Export ("setSelectedRanges:affinity:stillSelecting:")]
		void SetSelectedRanges (NSArray /*NSRange []*/ ranges, NSSelectionAffinity affinity, bool stillSelectingFlag);

		[Export ("setSelectedRange:affinity:stillSelecting:")]
		void SetSelectedRange (NSRange charRange, NSSelectionAffinity affinity, bool stillSelectingFlag);

		[Export ("selectionAffinity")]
		NSSelectionAffinity SelectionAffinity ();

		[Export ("updateInsertionPointStateAndRestartTimer:")]
		void UpdateInsertionPointStateAndRestartTimer (bool restartFlag);

		[Export ("toggleContinuousSpellChecking:")]
		void ToggleContinuousSpellChecking (NSObject sender);

		[Export ("spellCheckerDocumentTag")]
		nint SpellCheckerDocumentTag ();

		[Export ("toggleGrammarChecking:")]
		void ToggleGrammarChecking (NSObject sender);

		[Export ("setSpellingState:range:")]
		void SetSpellingState (nint value, NSRange charRange);

		[Export ("shouldChangeTextInRanges:replacementStrings:")]
		bool ShouldChangeText (NSArray /* NSRange [] */ affectedRanges, string [] replacementStrings);

		[Export ("rangesForUserTextChange")]
		NSArray /* NSRange [] */ RangesForUserTextChange ();

		[Export ("rangesForUserCharacterAttributeChange")]
		NSArray /* NSRange [] */ RangesForUserCharacterAttributeChange ();

		[Export ("rangesForUserParagraphAttributeChange")]
		NSArray /* NSRange [] */ RangesForUserParagraphAttributeChange ();

		//[Export ("shouldChangeTextInRange:replacementString:")]
		//bool ShouldChangeText (NSRange affectedCharRange, string replacementString);

		[Export ("rangeForUserTextChange")]
		NSRange RangeForUserTextChange ();

		[Export ("rangeForUserCharacterAttributeChange")]
		NSRange RangeForUserCharacterAttributeChange ();

		[Export ("rangeForUserParagraphAttributeChange")]
		NSRange RangeForUserParagraphAttributeChange ();

		[Export ("breakUndoCoalescing")]
		void BreakUndoCoalescing ();

		[Export ("isCoalescingUndo")]
		bool IsCoalescingUndo ();

		[Export ("showFindIndicatorForRange:")]
		void ShowFindIndicatorForRange (NSRange charRange);

		[Export ("setSelectedRange:")]
		void SetSelectedRange (NSRange charRange);

		//Detected properties
		[Export ("selectedRanges")]
		NSArray /* NSRange [] */ SelectedRanges { get; set; }

		[Export ("selectionGranularity")]
		NSSelectionGranularity SelectionGranularity { get; set; }

		[Export ("selectedTextAttributes")]
		NSDictionary SelectedTextAttributes { get; set; }

		[Export ("insertionPointColor")]
		NSColor InsertionPointColor { get; set; }

		[Export ("markedTextAttributes")]
		NSDictionary MarkedTextAttributes { get; set; }

		[Export ("linkTextAttributes")]
		NSDictionary LinkTextAttributes { get; set; }

		[Export ("displaysLinkToolTips")]
		bool DisplaysLinkToolTips { get; set; }

		[Export ("acceptsGlyphInfo")]
		bool AcceptsGlyphInfo { get; set; }

		[Export ("rulerVisible")]
		bool RulerVisible { [Bind ("isRulerVisible")]get; set; }

		[Export ("usesRuler")]
		bool UsesRuler { get; set; }

		[Export ("continuousSpellCheckingEnabled")]
		bool ContinuousSpellCheckingEnabled { [Bind ("isContinuousSpellCheckingEnabled")]get; set; }

		[Export ("grammarCheckingEnabled")]
		bool GrammarCheckingEnabled { [Bind ("isGrammarCheckingEnabled")]get; set; }

		[Export ("typingAttributes")]
		NSDictionary TypingAttributes { get; set; }

		[Export ("usesFindPanel")]
		bool UsesFindPanel { get; set; }

		[Export ("allowsDocumentBackgroundColorChange")]
		bool AllowsDocumentBackgroundColorChange { get; set; }

		[Export ("defaultParagraphStyle")]
		NSParagraphStyle DefaultParagraphStyle { get; set; }

		[Export ("allowsUndo")]
		bool AllowsUndo { get; set; }

		[Export ("allowsImageEditing")]
		bool AllowsImageEditing { get; set; }

		[Export ("delegate")]
		NSTextViewDelegate Delegate { get; set; }

		[Export ("editable")]
		bool Editable { [Bind ("isEditable")]get; set; }

		[Export ("selectable")]
		bool Selectable { [Bind ("isSelectable")]get; set; }

		[Export ("richText")]
		bool RichText { [Bind ("isRichText")]get; set; }

		[Export ("importsGraphics")]
		bool ImportsGraphics { get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("fieldEditor")]
		bool FieldEditor { [Bind ("isFieldEditor")]get; set; }

		[Export ("usesFontPanel")]
		bool UsesFontPanel { get; set; }

		[Export ("allowedInputSourceLocales")]
		string [] AllowedInputSourceLocales { get; set; }

		// FIXME: binding
		//[Export ("shouldChangeTextInRanges:replacementStrings:")]
		//bool ShouldChangeTextInRanges (NSArray affectedRanges, NSArray replacementStrings);

		// FIXME: binding
		//[Export ("rangesForUserTextChange")]
		//NSArray RangesForUserTextChange ();

		// FIXME: binding
		//[Export ("rangesForUserCharacterAttributeChange")]
		//NSArray RangesForUserCharacterAttributeChange ();

		// FIXME: binding
		//[Export ("rangesForUserParagraphAttributeChange")]
		//NSArray RangesForUserParagraphAttributeChange ();

		[Export ("shouldChangeTextInRange:replacementString:")]
		bool ShouldChangeText (NSRange affectedCharRange, string replacementString);

		[Export ("didChangeText")]
		void DidChangeText ();

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		//
		// Smart copy/paset support
		//
		[Export ("smartDeleteRangeForProposedRange:")]
		NSRange SmartDeleteRangeForProposedRange (NSRange proposedCharRange);

		[Export ("toggleSmartInsertDelete:")]
		void ToggleSmartInsertDelete (NSObject sender);

		[Export ("smartInsertForString:replacingRange:beforeString:afterString:")]
		void SmartInsert (string pasteString, NSRange charRangeToReplace, string beforeString, string afterString);

		[Export ("smartInsertBeforeStringForString:replacingRange:")]
		string SmartInsertBefore (string pasteString, NSRange charRangeToReplace);

		[Export ("smartInsertAfterStringForString:replacingRange:")]
		string SmartInsertAfter (string pasteString, NSRange charRangeToReplace);

		[Export ("toggleAutomaticQuoteSubstitution:")]
		void ToggleAutomaticQuoteSubstitution (NSObject sender);

		[Export ("toggleAutomaticLinkDetection:")]
		void ToggleAutomaticLinkDetection (NSObject sender);

		[Export ("toggleAutomaticDataDetection:")]
		void ToggleAutomaticDataDetection (NSObject sender);

		[Export ("toggleAutomaticDashSubstitution:")]
		void ToggleAutomaticDashSubstitution (NSObject sender);

		[Export ("toggleAutomaticTextReplacement:")]
		void ToggleAutomaticTextReplacement (NSObject sender);

		[Export ("toggleAutomaticSpellingCorrection:")]
		void ToggleAutomaticSpellingCorrection (NSObject sender);

		[Export ("checkTextInRange:types:options:")]
		void CheckText (NSRange range, NSTextCheckingTypes checkingTypes, NSDictionary options);

		[Export ("handleTextCheckingResults:forRange:types:options:orthography:wordCount:")]
		void HandleTextChecking (NSTextCheckingResult [] results, NSRange range, NSTextCheckingTypes checkingTypes, NSDictionary options, NSOrthography orthography, nint wordCount);

		[Export ("orderFrontSubstitutionsPanel:")]
		void OrderFrontSubstitutionsPanel (NSObject sender);

		[Export ("checkTextInSelection:")]
		void CheckTextInSelection (NSObject sender);

		[Export ("checkTextInDocument:")]
		void CheckTextInDocument (NSObject sender);

		//Detected properties
		[Export ("smartInsertDeleteEnabled")]
		bool SmartInsertDeleteEnabled { get; set; }

		[Export ("automaticQuoteSubstitutionEnabled")]
		bool AutomaticQuoteSubstitutionEnabled { [Bind ("isAutomaticQuoteSubstitutionEnabled")]get; set; }

		[Export ("automaticLinkDetectionEnabled")]
		bool AutomaticLinkDetectionEnabled { [Bind ("isAutomaticLinkDetectionEnabled")]get; set; }

		[Export ("automaticDataDetectionEnabled")]
		bool AutomaticDataDetectionEnabled { [Bind ("isAutomaticDataDetectionEnabled")]get; set; }

		[Export ("automaticDashSubstitutionEnabled")]
		bool AutomaticDashSubstitutionEnabled { [Bind ("isAutomaticDashSubstitutionEnabled")]get; set; }

		[Export ("automaticTextReplacementEnabled")]
		bool AutomaticTextReplacementEnabled { [Bind ("isAutomaticTextReplacementEnabled")]get; set; }

		[Export ("automaticSpellingCorrectionEnabled")]
		bool AutomaticSpellingCorrectionEnabled { [Bind ("isAutomaticSpellingCorrectionEnabled")]get; set; }

		[Export ("enabledTextCheckingTypes")]
		NSTextCheckingTypes EnabledTextCheckingTypes { get; set; }
	}

	//64 bit reviewed
	[BaseType (typeof (NSTextDelegate))]
	[Model]
	public partial interface NSTextViewDelegate {
		[Export ("textView:clickedOnLink:atIndex:"), DelegateName ("NSTextViewLink"), DefaultValue (false)]
		bool LinkClicked (NSTextView textView, NSObject link, nuint charIndex);

		[Export ("textView:clickedOnCell:inRect:atIndex:"), EventArgs ("NSTextViewClicked")]
		void CellClicked (NSTextView textView, NSTextAttachmentCell cell, CGRect cellFrame, nuint charIndex);

		[Export ("textView:doubleClickedOnCell:inRect:atIndex:"), EventArgs ("NSTextViewDoubleClick")]
		void CellDoubleClicked (NSTextView textView, NSTextAttachmentCell cell, CGRect cellFrame, nuint charIndex);

		[Export ("textView:writablePasteboardTypesForCell:atIndex:"), DelegateName ("NSTextViewCellPosition"),DefaultValue (null)]
		string [] GetWritablePasteboardTypes (NSTextView view, NSTextAttachmentCell forCell, nuint charIndex);

		[Export ("textView:writeCell:atIndex:toPasteboard:type:"), DelegateName ("NSTextViewCellPasteboard"), DefaultValue (true)]
		bool WriteCell (NSTextView view, NSTextAttachmentCell cell, uint charIndex, NSPasteboard pboard, string type);

		[Export ("textView:willChangeSelectionFromCharacterRange:toCharacterRange:"), DelegateName ("NSTextViewSelectionChange"), DefaultValueFromArgument ("newSelectedCharRange")]
		NSRange WillChangeSelection (NSTextView textView, NSRange oldSelectedCharRange, NSRange newSelectedCharRange);

		[Export ("textView:willChangeSelectionFromCharacterRanges:toCharacterRanges:"), DelegateName ("NSTextViewSelectionWillChange"), DefaultValueFromArgument ("newSelectedCharRanges")]
		NSValue [] WillChangeSelectionFromRanges (NSTextView textView, NSValue [] oldSelectedCharRanges, NSValue [] newSelectedCharRanges);

		[Export ("textView:shouldChangeTextInRanges:replacementStrings:"), DelegateName ("NSTextViewSelectionShouldChange"), DefaultValueFromArgument ("null")]
		bool ShouldChangeTextInRanges (NSTextView textView, NSValue [] affectedRanges, string [] replacementStrings);

		[Export ("textView:shouldChangeTypingAttributes:toAttributes:"), DelegateName ("NSTextViewTypeAttribute"), DefaultValueFromArgument ("newTypingAttributes")]
		NSDictionary ShouldChangeTypingAttributes (NSTextView textView, NSDictionary oldTypingAttributes, NSDictionary newTypingAttributes);

		[Export ("textViewDidChangeSelection:"), EventArgs ("NSTextViewNotification")]
		void DidChangeSelection (NSNotification notification);

		[Export ("textViewDidChangeTypingAttributes:"), EventArgs ("NSTextViewNotification")]
		void DidChangeTypingAttributes (NSNotification notification);

		[Export ("textView:willDisplayToolTip:forCharacterAtIndex:"), DelegateName ("NSTextViewTooltip"), DefaultValueFromArgument ("tooltip")]
		string WillDisplayToolTip (NSTextView textView, string tooltip, nuint characterIndex);

		[Export ("textView:completions:forPartialWordRange:indexOfSelectedItem:"), DelegateName ("NSTextViewCompletion"), DefaultValue (null)]
		string [] GetCompletions (NSTextView textView, string [] words, NSRange charRange, nint index);

		[Export ("textView:shouldChangeTextInRange:replacementString:"), DelegateName ("NSTextViewChangeText"), DefaultValue (true)]
		bool ShouldChangeTextInRange (NSTextView textView, NSRange affectedCharRange, string replacementString);

		[Export ("textView:doCommandBySelector:"), DelegateName ("NSTextViewSelectorCommand"), DefaultValue (false)]
		bool DoCommandBySelector (NSTextView textView, Selector commandSelector);

		[Export ("textView:shouldSetSpellingState:range:"), DelegateName ("NSTextViewSpellingQuery"), DefaultValue (0)]
		nint ShouldSetSpellingState (NSTextView textView, nint value, NSRange affectedCharRange);

		[Export ("textView:menu:forEvent:atIndex:"), DelegateName ("NSTextViewEventMenu"), DefaultValueFromArgument ("menu")]
		NSMenu MenuForEvent (NSTextView view, NSMenu menu, NSEvent theEvent, nuint charIndex);

		[Export ("textView:willCheckTextInRange:options:types:"), DelegateName ("NSTextViewOnTextCheck"), DefaultValueFromArgument ("options")]
		NSDictionary WillCheckText (NSTextView view, NSRange range, NSDictionary options, NSTextCheckingTypes checkingTypes);

		[Export ("textView:didCheckTextInRange:types:options:results:orthography:wordCount:"), DelegateName ("NSTextViewTextChecked"), DefaultValueFromArgument ("results")]
		NSTextCheckingResult [] DidCheckText (NSTextView view, NSRange range, NSTextCheckingTypes checkingTypes, NSDictionary options, NSTextCheckingResult [] results, NSOrthography orthography, nint wordCount);

		[Export ("textView:draggedCell:inRect:event:"), EventArgs ("NSTextViewDraggedCell")]
		void DraggedCell (NSTextView view, NSTextAttachmentCell cell, CGRect rect, NSEvent theevent);

		[Export ("undoManagerForTextView:"), DelegateName ("NSTextViewGetUndoManager"), DefaultValue (null)]
		NSUndoManager GetUndoManager (NSTextView view);
	}
	
	//64 bit reviewed
	[BaseType (typeof (NSTextField))]
	public interface NSTokenField {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("tokenStyle")]
		NSTokenStyle TokenStyle { get; set; }

		[Export ("completionDelay")]
		double CompletionDelay { get; set; }

		[Static]
		[Export ("defaultCompletionDelay")]
		double DefaultCompletionDelay { get; }

		[Static]
		[Export ("defaultTokenizingCharacterSet")]
		NSCharacterSet DefaultCharacterSet { get; }

		//Detected properties
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSTokenFieldDelegate Delegate { get; set; }

		[Export ("tokenizingCharacterSet")]
		NSCharacterSet CharacterSet { get; set; }
	}

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSTokenFieldDelegate {
		[Export ("tokenField:completionsForSubstring:indexOfToken:indexOfSelectedItem:")]
		string [] GetCompletionStrings (NSTokenField tokenField, string substring, nint tokenIndex, nint selectedIndex);

		[Export ("tokenField:shouldAddObjects:atIndex:")]
		NSTokenField [] ShouldAddObjects (NSTokenField tokenField, NSTokenField [] tokens, uint index);

		[Export ("tokenField:displayStringForRepresentedObject:")]
		string GetDisplayString (NSTokenField tokenField, NSObject representedObject);

		[Export ("tokenField:editingStringForRepresentedObject:")]
		string GetEditingString (NSTokenField tokenField, NSObject representedObject);

		[Export ("tokenField:representedObjectForEditingString:")]
		NSObject GetRepresentedObject (NSTokenField tokenField, string editingString);

		[Export ("tokenField:writeRepresentedObjects:toPasteboard:")]
		bool WriteRepresented (NSTokenField tokenField, NSArray objects, NSPasteboard pboard);

		[Export ("tokenField:readFromPasteboard:")]
		NSObject [] Read (NSTokenField tokenField, NSPasteboard pboard);

		[Export ("tokenField:menuForRepresentedObject:")]
		NSMenu GetMenu (NSTokenField tokenField, NSObject representedObject);

		[Export ("tokenField:hasMenuForRepresentedObject:")]
		bool HasMenu (NSTokenField tokenField, NSObject representedObject);

		[Export ("tokenField:styleForRepresentedObject:")]
		NSTokenStyle GetStyle (NSTokenField tokenField, NSObject representedObject);

	}

	// 64 bit reviewed
	[BaseType (typeof (NSObject), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSToolbarDelegate)})]
	public partial interface NSToolbar {
		[Export ("initWithIdentifier:")]
		IntPtr Constructor (string identifier);

		[Export ("insertItemWithItemIdentifier:atIndex:")]
		void InsertItem (string itemIdentifier, nint index);

		[Export ("removeItemAtIndex:")]
		void RemoveItem (nint index);

		[Export ("runCustomizationPalette:")]
		void RunCustomizationPalette (NSObject sender);

		[Export ("customizationPaletteIsRunning")]
		bool IsCustomizationPaletteRunning { get; }

		[Export ("identifier")]
		string Identifier { get; }

		[Export ("items")]
		NSToolbarItem [] Items { get; }

		[Export ("visibleItems")]
		NSToolbarItem [] VisibleItems { get; }

		[Export ("setConfigurationFromDictionary:")]
		void SetConfigurationFromDictionary (NSDictionary configDict);

		[Export ("configurationDictionary")]
		NSDictionary ConfigurationDictionary { get; }

		[Export ("validateVisibleItems")]
		void ValidateVisibleItems ();

		//Detected properties
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSToolbarDelegate Delegate { get; set; }

		[Export ("visible")]
		bool Visible { [Bind ("isVisible")]get; set; }

		[Export ("displayMode")]
		NSToolbarDisplayMode DisplayMode { get; set; }

		[Export ("selectedItemIdentifier"), NullAllowed]
		string SelectedItemIdentifier { get; set; }

		[Export ("sizeMode")]
		NSToolbarSizeMode SizeMode { get; set; }

		[Export ("showsBaselineSeparator")]
		bool ShowsBaselineSeparator { get; set; }

		[Export ("allowsUserCustomization")]
		bool AllowsUserCustomization { get; set; }

		[Export ("autosavesConfiguration")]
		bool AutosavesConfiguration { get; set; }

		[Field ("NSToolbarSeparatorItemIdentifier")]
		NSString NSToolbarSeparatorItemIdentifier { get; }
		
		[Field ("NSToolbarSpaceItemIdentifier")]
		NSString NSToolbarSpaceItemIdentifier { get; }
		
		[Field ("NSToolbarFlexibleSpaceItemIdentifier")]
		NSString NSToolbarFlexibleSpaceItemIdentifier { get; }
		
		[Field ("NSToolbarShowColorsItemIdentifier")]
		NSString NSToolbarShowColorsItemIdentifier { get; }
		
		[Field ("NSToolbarShowFontsItemIdentifier")]
		NSString NSToolbarShowFontsItemIdentifier { get; }
		
		[Field ("NSToolbarCustomizeToolbarItemIdentifier")]
		NSString NSToolbarCustomizeToolbarItemIdentifier { get; }
		
		[Field ("NSToolbarPrintItemIdentifier")]
		NSString NSToolbarPrintItemIdentifier { get; }
	}

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSToolbarDelegate {
		[Abstract]
		[Export ("toolbar:itemForItemIdentifier:willBeInsertedIntoToolbar:"), DelegateName ("NSToolbarWillInsert"), DefaultValue (null)]
		NSToolbarItem WillInsertItem (NSToolbar toolbar, string itemIdentifier, bool willBeInserted);

		[Abstract]
		[Export ("toolbarDefaultItemIdentifiers:"), DelegateName ("NSToolbarIdentifiers"), DefaultValue (null)]
		string [] DefaultItemIdentifiers (NSToolbar toolbar);

		[Abstract]
		[Export ("toolbarAllowedItemIdentifiers:"), DelegateName ("NSToolbarIdentifiers"), DefaultValue (null)]
		string [] AllowedItemIdentifiers (NSToolbar toolbar);

		[Abstract]
		[Export ("toolbarSelectableItemIdentifiers:"), DelegateName ("NSToolbarIdentifiers"), DefaultValue (null)]
		string [] SelectableItemIdentifiers (NSToolbar toolbar);

		[Abstract]
		[Export ("toolbarWillAddItem:"), EventArgs ("NSNotification")]
		void WillAddItem (NSNotification notification);

		[Abstract]
		[Export ("toolbarDidRemoveItem:"), EventArgs ("NSNotification")]
		void DidRemoveItem (NSNotification notification);
	}

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	public interface NSToolbarItem {
		[Export ("initWithItemIdentifier:")]
		IntPtr Constructor (string itemIdentifier);

		[Export ("itemIdentifier")]
		string Identifier { get; }

		[Export ("toolbar")]
		NSToolbar Toolbar { get; }

		[Export ("validate")]
		void Validate ();

		[Export ("allowsDuplicatesInToolbar")]
		bool AllowsDuplicatesInToolbar { get; }

		//Detected properties
		[Export ("label")]
		string Label { get; set; }

		[Export ("paletteLabel")]
		string PaletteLabel { get; set; }

		[Export ("toolTip")]
		string ToolTip { get; set; }

		[Export ("menuFormRepresentation")]
		NSMenuItem MenuFormRepresentation { get; set; }

		[Export ("tag")]
		nint Tag { get; set; }

		[Export ("target"), NullAllowed]
		NSObject Target { get; set; }

		[Export ("action"), NullAllowed]
		Selector Action { get; set; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")]get; set; }

		[Export ("image"), NullAllowed]
		NSImage Image { get; set; }

		[Export ("view")]
		NSView View { get; set; }

		[Export ("minSize")]
		CGSize MinSize { get; set; }

		[Export ("maxSize")]
		CGSize MaxSize { get; set; }

		[Export ("visibilityPriority")]
		nint VisibilityPriority { get; set; }

		[Export ("autovalidates")]
		bool Autovalidates { get; set; }
	}

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	public interface NSTouch {
		[Export ("identity", ArgumentSemantic.Retain)]
		NSObject Identity { get; }

		[Export ("phase")]
		NSTouchPhase Phase { get; }

		[Export ("normalizedPosition")]
		CGPoint NormalizedPosition { get; }

		[Export ("isResting")]
		bool IsResting { get; }

		[Export ("device", ArgumentSemantic.Retain)]
		NSObject Device { get; }

		[Export ("deviceSize")]
		CGSize DeviceSize { get; }
	}

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	public interface NSTrackingArea {
		[Export ("initWithRect:options:owner:userInfo:")]
		IntPtr Constructor (CGRect rect, NSTrackingAreaOptions options, NSObject owner, [NullAllowed] NSDictionary userInfo);

		[Export ("rect")]
		CGRect Rect { get; }

		[Export ("options")]
		NSTrackingAreaOptions Options { get; }

		[Export ("owner")]
		NSObject Owner { get; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; }
	}

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	public interface NSTreeNode {
		[Static, Export ("treeNodeWithRepresentedObject:")]
		NSTreeNode FromRepresentedObject (NSObject modelObject);

		[Export ("initWithRepresentedObject:")]
		IntPtr Constructor (NSObject modelObject);

		[Export ("representedObject")]
		NSObject RepresentedObject { get; }

		[Export ("indexPath")]
		NSIndexPath IndexPath { get; }

		[Export ("isLeaf")]
		bool IsLeaf { get; }

		[Export ("childNodes")]
		NSTreeNode [] Children { get; }

		//[Export ("mutableChildNodes")]
		//NSMutableArray MutableChildren { get; }

		[Export ("descendantNodeAtIndexPath:")]
		NSTreeNode DescendantNode (NSIndexPath atIndexPath);

		[Export ("parentNode")]
		NSTreeNode ParentNode { get; }

		[Export ("sortWithSortDescriptors:recursively:")]
		void SortWithSortDescriptors (NSSortDescriptor [] sortDescriptors, bool recursively);

	}

	// 64 bit reviewed
	[BaseType (typeof (NSObjectController))]
	public interface NSTreeController {
		[Export ("rearrangeObjects")]
		void RearrangeObjects ();

		[Export ("arrangedObjects")]
		NSObject ArrangedObjects { get; }

		[Export ("childrenKeyPath")]
		string ChildrenKeyPath { get; set; }

		[Export ("countKeyPath")]
		string CountKeyPath { get; set; }

		[Export ("leafKeyPath")]
		string LeafKeyPath { get; set; }

		[Export ("sortDescriptors")]
		NSSortDescriptor [] SortDescriptors { get; set; }

		[Export ("content")]
		NSObject Content { get; set; }

		[Export ("add:")]
		void Add (NSObject sender);

		[Export ("remove:")]
		void Remove (NSObject sender);

		[Export ("addChild:")]
		void AddChild (NSObject sender);

		[Export ("insert:")]
		void Insert (NSObject sender);

		[Export ("insertChild:")]
		void InsertChild (NSObject sender);

		[Export ("canInsert")]
		bool CanInsert { get; }

		[Export ("canInsertChild")]
		bool CanInsertChild { get; }

		[Export ("canAddChild")]
		bool CanAddChild { get; }

		[Export ("insertObject:atArrangedObjectIndexPath:")]
		void InsertObject (NSObject object1, NSIndexPath indexPath);

		[Export ("insertObjects:atArrangedObjectIndexPaths:")]
		void InsertObjects (NSObject [] objects, NSArray indexPaths);

		[Export ("removeObjectAtArrangedObjectIndexPath:")]
		void RemoveObjectAtArrangedObjectIndexPath (NSIndexPath indexPath);

		[Export ("removeObjectsAtArrangedObjectIndexPaths:")]
		void RemoveObjectsAtArrangedObjectIndexPaths (NSIndexPath [] indexPaths);

		[Export ("avoidsEmptySelection")]
		bool AvoidsEmptySelection { get; set; }

		[Export ("preservesSelection")]
		bool PreservesSelection { get; set; }

		[Export ("selectsInsertedObjects")]
		bool SelectsInsertedObjects { get; set; }

		[Export ("alwaysUsesMultipleValuesMarker")]
		bool AlwaysUsesMultipleValuesMarker { get; set; }

		[Export ("selectedObjects")]
		NSObject [] SelectedObjects { get; }

		[Export ("selectionIndexPaths"), Protected]
		NSIndexPath [] GetSelectionIndexPaths ();

		[Export ("setSelectionIndexPaths:"), Protected]
		bool SetSelectionIndexPaths (NSIndexPath [] indexPaths);

		[Export ("selectionIndexPath"), Protected]
		NSIndexPath GetSelectionIndexPath ();

		[Export ("setSelectionIndexPath:"), Protected]
		bool SetSelectionIndexPath (NSIndexPath index);

		[Export ("addSelectionIndexPaths:")]
		bool AddSelectionIndexPaths (NSIndexPath [] indexPaths);

		[Export ("removeSelectionIndexPaths:")]
		bool RemoveSelectionIndexPaths (NSIndexPath [] indexPaths);

		[Export ("selectedNodes")]
		NSTreeNode [] SelectedNodes { get; }

		[Export ("moveNode:toIndexPath:")]
		void MoveNode (NSTreeNode node, NSIndexPath indexPath);

		[Export ("moveNodes:toIndexPath:")]
		void MoveNodes (NSTreeNode [] nodes, NSIndexPath startingIndexPath);

		[Export ("childrenKeyPathForNode:")]
		string ChildrenKeyPathForNode (NSTreeNode node);

		[Export ("countKeyPathForNode:")]
		string CountKeyPathForNode (NSTreeNode node);

		[Export ("leafKeyPathForNode:")]
		string LeafKeyPathForNode (NSTreeNode node);
	}

	[BaseType (typeof (NSObject))]
	public partial interface NSTypesetter {

	}

	interface NSWindowBackingPropertiesEventArgs {
		[Export ("NSBackingPropertyOldScaleFactorKey")]
		nint OldScaleFactor { get; }

		[Export ("NSBackingPropertyOldColorSpaceKey")]
		NSColorSpace OldColorSpace { get; }
	}
	
	
	public delegate void NSWindowTrackEventsMatchingCompletionHandler (NSEvent evt, ref bool stop);
	
	//64 bit reviewed
	[BaseType (typeof (NSResponder), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSWindowDelegate)})]
	public partial interface NSWindow : NSAnimatablePropertyContainer, NSUserInterfaceItemIdentification, NSAppearanceCustomization {
		[Static, Export ("frameRectForContentRect:styleMask:")]
		CGRect FrameRectFor (CGRect contectRect, NSWindowStyle styleMask);
	
		[Static]
		[Export ("contentRectForFrameRect:styleMask:")]
		CGRect ContentRectFor (CGRect forFrameRect, NSWindowStyle styleMask);
	
		[Static]
		[Export ("minFrameWidthWithTitle:styleMask:")]
		nfloat MinFrameWidthWithTitle (string aTitle, NSWindowStyle aStyle);
	
		[Static]
		[Export ("defaultDepthLimit")]
		NSWindowDepth DefaultDepthLimit { get; }
	
		[Export ("frameRectForContentRect:")]
		CGRect FrameRectFor (CGRect contentRect);
	
		[Export ("contentRectForFrameRect:")]
		CGRect ContentRectFor (CGRect frameRect);
	
		[Export ("initWithContentRect:styleMask:backing:defer:")]
		IntPtr Constructor (CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation);
	
		[Export ("initWithContentRect:styleMask:backing:defer:screen:")]
		IntPtr Constructor (CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation, NSScreen  screen);
	
		[Export ("title")]
		string Title  { get; set; }
	
		[Export ("representedURL")]
		NSUrl RepresentedUrl { get; set; }
	
		[Export ("representedFilename")]
		string RepresentedFilename  { get; set; }
	
		[Export ("setTitleWithRepresentedFilename:")]
		void SetTitleWithRepresentedFilename (string  filename);
	
		[Export ("setExcludedFromWindowsMenu:")]
		void SetExcludedFromWindowsMenu (bool flag);
	
		[Export ("isExcludedFromWindowsMenu")]
		bool ExcludedFromWindowsMenu { get; } 
	
		[Export ("contentView")]
		NSView ContentView  { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }
	
		[Wrap ("WeakDelegate")][NullAllowed]
		NSWindowDelegate Delegate { get; set; }
	
		[Export ("windowNumber")]
		nint WindowNumber { get; }
	
		[Export ("styleMask")]
		NSWindowStyle StyleMask { get; set; }
	
		[Export ("fieldEditor:forObject:")]
		NSText FieldEditor (bool createFlag, NSObject forObject);
	
		[Export ("endEditingFor:")]
		void EndEditingFor ([NullAllowed] NSObject anObject);
	
		[Export ("constrainFrameRect:toScreen:")]
		CGRect ConstrainFrameRect (CGRect frameRect, [NullAllowed] NSScreen screen);
	
		[Export ("setFrame:display:")]
		void SetFrame (CGRect frameRect, bool display);
	
		[Export ("setContentSize:")]
		void SetContentSize (CGSize aSize);
	
		[Export ("setFrameOrigin:")]
		void SetFrameOrigin (CGPoint aPoint);
	
		[Export ("setFrameTopLeftPoint:")]
		void SetFrameTopLeftPoint (CGPoint aPoint);
	
		[Export ("cascadeTopLeftFromPoint:")]
		CGPoint CascadeTopLeftFromPoint (CGPoint topLeftPoint);
	
		[Export ("frame")]
		CGRect Frame { get; }
	
		[Export ("animationResizeTime:")]
		double AnimationResizeTime (CGRect newFrame);
	
		[Export ("setFrame:display:animate:")]
		void SetFrame (CGRect frameRect, bool display, bool animate);
	
		[Export ("inLiveResize")]
		bool InLiveResize { get; } 
	
		[Export ("showsResizeIndicator")]
		bool ShowsResizeIndicator { get; set; }
	
		[Export ("resizeIncrements")]
		CGSize ResizeIncrements  { get; set; }
	
		[Export ("aspectRatio")]
		CGSize AspectRatio  { get; set; }
	
		[Export ("contentResizeIncrements")]
		CGSize ContentResizeIncrements  { get; set; }
	
		[Export ("contentAspectRatio")]
		CGSize ContentAspectRatio  { get; set; }
	
		[Export ("useOptimizedDrawing:")]
		void UseOptimizedDrawing (bool flag);
	
		[Export ("disableFlushWindow")]
		void DisableFlushWindow ();
	
		[Export ("enableFlushWindow")]
		void EnableFlushWindow ();
	
		[Export ("isFlushWindowDisabled")]
		bool FlushWindowDisabled { get; }
	
		[Export ("flushWindow")]
		void FlushWindow ();
	
		[Export ("flushWindowIfNeeded")]
		void FlushWindowIfNeeded ();
	
		[Export ("viewsNeedDisplay")]
		bool ViewsNeedDisplay  { get; set; }
	
		[Export ("displayIfNeeded")]
		void DisplayIfNeeded ();
	
		[Export ("display")]
		void Display ();
	
		[Export ("autodisplay")]
		bool Autodisplay  { [Bind ("isAutodisplay")] get; set; }
	
		[Export ("preservesContentDuringLiveResize")]
		bool PreservesContentDuringLiveResize  { get; set; }
	
		[Export ("update")]
		void Update ();
	
		[Export ("makeFirstResponder:")]
		bool MakeFirstResponder ([NullAllowed] NSResponder  aResponder);
	
		[Export ("firstResponder")]
		NSResponder FirstResponder { get; }
	
		[Export ("resizeFlags")]
		nint ResizeFlags { get; }
	
		[Export ("keyDown:")]
		void KeyDown (NSEvent  theEvent);
	
		/* NSWindow.Close by default calls [window release]
		 * This will cause a double free in our code since we're not aware of this
		 * and we end up GCing the proxy eventually and sending our own release
		 */
		[Internal, Export ("close")]
		void _Close ();
	
		[Export ("releasedWhenClosed")]
		bool ReleasedWhenClosed  { [Bind ("isReleasedWhenClosed")] get; set; }
	
		[Export ("miniaturize:")]
		void Miniaturize (NSObject sender);
	
		[Export ("deminiaturize:")]
		void Deminiaturize (NSObject sender);
	
		[Export ("isZoomed")]
		bool IsZoomed { get; set; }
	
		[Export ("zoom:")]
		void Zoom (NSObject sender);
	
		[Export ("isMiniaturized")]
		bool IsMiniaturized { get; set; }
	
		[Export ("tryToPerform:with:")]
		bool TryToPerform (Selector anAction, NSObject anObject);
		
		[Export ("validRequestorForSendType:returnType:")]
		NSObject ValidRequestorForSendType (string sendType, string returnType);
	
		[Export ("backgroundColor")]
		NSColor BackgroundColor  { get; set; }
	
		[Export ("setContentBorderThickness:forEdge:")]
		void SetContentBorderThickness (nfloat thickness, NSRectEdge edge);
	
		[Export ("contentBorderThicknessForEdge:")]
		nfloat ContentBorderThicknessForEdge (NSRectEdge edge);
	
		[Export ("setAutorecalculatesContentBorderThickness:forEdge:")]
		void SetAutorecalculatesContentBorderThickness (bool flag, NSRectEdge forEdge);
	
		[Export ("autorecalculatesContentBorderThicknessForEdge:")]
		bool AutorecalculatesContentBorderThickness (NSRectEdge forEdgeedge);
	
		[Export ("movable")]
		bool IsMovable  { [Bind ("isMovable")] get; set; }
	
		[Export ("movableByWindowBackground")]
		bool MovableByWindowBackground  { [Bind ("isMovableByWindowBackground")] get; set; }
	
		[Export ("hidesOnDeactivate")]
		bool HidesOnDeactivate  { get; set; }
	
		[Export ("canHide")]
		bool CanHide  { get; set; }
	
		[Export ("center")]
		void Center ();
	
		[Export ("makeKeyAndOrderFront:")]
		void MakeKeyAndOrderFront ([NullAllowed] NSObject sender);
	
		[Export ("orderFront:")]
		void OrderFront ([NullAllowed] NSObject sender);
		
		[Export ("orderBack:")]
		void OrderBack ([NullAllowed] NSObject sender);
	
		[Export ("orderOut:")]
		void OrderOut ([NullAllowed] NSObject sender);
	
		[Export ("orderWindow:relativeTo:")]
		void OrderWindow (NSWindowOrderingMode place, nint relativeTo);
	
		[Export ("orderFrontRegardless")]
		void OrderFrontRegardless ();
	
		[Export ("miniwindowImage")]
		NSImage MiniWindowImage { get; set; }
	
		[Export ("miniwindowTitle")]
		string MiniWindowTitle  { get; set; }
	
		[Export ("dockTile")]
		NSDockTile DockTile { get; } 
	
		[Export ("documentEdited")]
		bool DocumentEdited  { [Bind ("isDocumentEdited")] get; set; }
	
		[Export ("isVisible")]
		bool IsVisible  { get; set; }
	
		[Export ("isKeyWindow")]
		bool IsKeyWindow { get; }
	
		[Export ("isMainWindow")]
		bool IsMainWindow { get; }
		
		[Export ("canBecomeKeyWindow")]
		bool CanBecomeKeyWindow { get; }
		
		[Export ("canBecomeMainWindow")]
		bool CanBecomeMainWindow { get; }
	
		[Export ("makeKeyWindow")]
		void MakeKeyWindow ();
	
		[Export ("makeMainWindow")]
		void MakeMainWindow ();
	
		[Export ("becomeKeyWindow")]
		void BecomeKeyWindow ();
		
		[Export ("resignKeyWindow")]
		void ResignKeyWindow ();
		
		[Export ("becomeMainWindow")]
		void BecomeMainWindow ();
	
		[Export ("resignMainWindow")]
		void ResignMainWindow ();
		
		[Export ("worksWhenModal")]
		bool WorksWhenModal ();
		
		[Export ("preventsApplicationTerminationWhenModal")]
		bool PreventsApplicationTerminationWhenModal  { get; set; }
	
		[Export ("convertBaseToScreen:")]
		CGPoint ConvertBaseToScreen (CGPoint aPoint);
	
		[Export ("convertScreenToBase:")]
		CGPoint ConvertScreenToBase (CGPoint aPoint);
	
		[Export ("performClose:")]
		void PerformClose (NSObject sender);
		
		[Export ("performMiniaturize:")]
		void PerformMiniaturize (NSObject sender);
	
		[Export ("performZoom:")]
		void PerformZoom (NSObject sender);
	
		[Export ("gState")]
		nint GState();
	
		[Export ("setOneShot:")]
		void SetOneShot (bool flag);
	
		[Export ("isOneShot")]
		bool IsOneShot { get; }
	
		[Export ("dataWithEPSInsideRect:")]
		NSData DataWithEpsInsideRect (CGRect rect);
	
		[Export ("dataWithPDFInsideRect:")]
		NSData DataWithPdfInsideRect (CGRect rect);

		[Export ("print:")]
		void Print (NSObject sender);
	
		[Export ("disableCursorRects")]
		void DisableCursorRects ();
	
		[Export ("enableCursorRects")]
		void EnableCursorRects ();
	
		[Export ("discardCursorRects")]
		void DiscardCursorRects ();
	
		[Export ("areCursorRectsEnabled")]
		bool AreCursorRectsEnabled { get; }
	
		[Export ("invalidateCursorRectsForView:")]
		void InvalidateCursorRectsForView (NSView  aView);
	
		[Export ("resetCursorRects")]
		void ResetCursorRects ();
	
		[Export ("allowsToolTipsWhenApplicationIsInactive")]
		bool AllowsToolTipsWhenApplicationIsInactive  { get; set; }
	
		[Export ("backingType")]
		NSBackingStore BackingType  { get; set; }
	
		[Export ("level")]
		NSWindowLevel Level  { get; set; }
	
		[Export ("depthLimit")]
		NSWindowDepth DepthLimit  { get; set; }
	
		[Export ("dynamicDepthLimit")]
		bool HasDynamicDepthLimit { [Bind ("hasDynamicDepthLimit")] get; set; }
	
		[Export ("screen")]
		NSScreen Screen { get; }
	
		[Export ("deepestScreen")]
		NSScreen DeepestScreen { get; }
	
		[Export ("canStoreColor")]
		bool CanStoreColor { get; }
	
		[Export ("hasShadow")]
		bool HasShadow  { get; set; }
	
		[Export ("invalidateShadow")]
		void InvalidateShadow ();
	
		[Export ("alphaValue")]
		nfloat AlphaValue  { get; set; }
	
		[Export ("opaque")]
		bool IsOpaque  { [Bind ("isOpaque")]get; set; }
	
		[Export ("sharingType")]
		NSWindowSharingType SharingType  { get; set; }
	
		[Export ("preferredBackingLocation")]
		NSWindowBackingLocation PreferredBackingLocation  { get; set; }
	
		[Export ("backingLocation")]
		NSWindowBackingLocation BackingLocation { get; }
	
		[Export ("allowsConcurrentViewDrawing")]
		bool AllowsConcurrentViewDrawing  { get; set; }
	
		[Export ("displaysWhenScreenProfileChanges")]
		bool DisplaysWhenScreenProfileChanges  { get; set; }
	
		[Export ("disableScreenUpdatesUntilFlush")]
		void DisableScreenUpdatesUntilFlush ();
	
		[Export ("canBecomeVisibleWithoutLogin")]
		bool CanBecomeVisibleWithoutLogin { get; set; }
	
		[Export ("collectionBehavior")]
		NSWindowCollectionBehavior CollectionBehavior  { get; set; }
	
		[Export ("isOnActiveSpace")]
		bool IsOnActiveSpace { get; }
	
		[Export ("stringWithSavedFrame")]
		string StringWithSavedFrame ();
	
		[Export ("setFrameFromString:")]
		void SetFrameFrom (string str);
	
		[Export ("saveFrameUsingName:")]
		void SaveFrameUsingName (string  name);
	
		[Export ("setFrameUsingName:force:")]
		bool SetFrameUsingName (string  name, bool force);
	
		[Export ("setFrameUsingName:")]
		bool SetFrameUsingName (string  name);
	
		[Export ("frameAutosaveName"), Protected]
		string GetFrameAutosaveName ();

		[Export ("setFrameAutosaveName:"), Protected]
		bool SetFrameAutosaveName (string frameName);

		[Static]
		[Export ("removeFrameUsingName:")]
		void RemoveFrameUsingName (string  name);
	
		[Export ("cacheImageInRect:")]
		void CacheImageInRect (CGRect aRect);
	
		[Export ("restoreCachedImage")]
		void RestoreCachedImage ();
	
		[Export ("discardCachedImage")]
		void DiscardCachedImage ();
	
		[Export ("minSize")]
		CGSize MinSize  { get; set; }
	
		[Export ("maxSize")]
		CGSize MaxSize  { get; set; }
	
		[Export ("contentMinSize")]
		CGSize ContentMinSize  { get; set; }
	
		[Export ("contentMaxSize")]
		CGSize ContentMaxSize  { get; set; }
	
		[Export ("nextEventMatchingMask:"), Protected]
		NSEvent NextEventMatchingMask (nuint mask);

		[Export ("nextEventMatchingMask:untilDate:inMode:dequeue:"), Protected]
		NSEvent NextEventMatchingMask (nuint mask, NSDate  expiration, string  mode, bool deqFlag);
	
		[Export ("discardEventsMatchingMask:beforeEvent:"), Protected]
		void DiscardEventsMatchingMask (nuint mask, NSEvent beforeLastEvent);

		[Export ("postEvent:atStart:")]
		void PostEvent (NSEvent theEvent, bool atStart);
	
		[Export ("currentEvent")]
		NSEvent CurrentEvent ();
	
		[Export ("acceptsMouseMovedEvents")]
		bool AcceptsMouseMovedEvents  { get; set; }
	
		[Export ("ignoresMouseEvents")]
		bool IgnoresMouseEvents  { get; set; }
	
		[Export ("deviceDescription")]
		NSDictionary DeviceDescription { get; }
	
		[Export ("sendEvent:")]
		void SendEvent (NSEvent  theEvent);
	
		[Export ("mouseLocationOutsideOfEventStream")]
		CGPoint MouseLocationOutsideOfEventStream { get; }
	
		[Static]
		[Export ("menuChanged:")]
		void MenuChanged (NSMenu  menu);
	
		[Export ("windowController")]
		NSObject WindowController { get; set; }
	
		[Export ("isSheet")]
		bool IsSheet { get; }
	
		[Export ("attachedSheet")]
		NSWindow AttachedSheet { get; }

		[Static]
		[Export ("standardWindowButton:forStyleMask:")]
		NSButton StandardWindowButton (NSWindowButton b, NSWindowStyle styleMask);
	
		[Export ("standardWindowButton:")]
		NSButton StandardWindowButton (NSWindowButton b);
	
		[Export ("addChildWindow:ordered:")][PostGet ("ChildWindows")]
		void AddChildWindow (NSWindow  childWin, NSWindowOrderingMode place);
	
		[Export ("removeChildWindow:")][PostGet ("ChildWindows")]
		void RemoveChildWindow (NSWindow  childWin);
	
		[Export ("childWindows")]
		NSWindow [] ChildWindows { get; }
	
		[Export ("parentWindow"), NullAllowed]
		NSWindow ParentWindow { get; set; }
	
		[Export ("graphicsContext")]
		NSGraphicsContext GraphicsContext { get; }
	
		[Export ("userSpaceScaleFactor")]
		nfloat UserSpaceScaleFactor { get; }
	
		[Export ("colorSpace")]
		NSColorSpace ColorSpace  { get; set; }
	
		[Static]
		[Export ("windowNumbersWithOptions:")]
		NSArray WindowNumbersWithOptions (NSWindowNumberListOptions options);
	
		[Static]
		[Export ("windowNumberAtPoint:belowWindowWithWindowNumber:")]
		nint WindowNumberAtPoint (CGPoint point, nint windowNumber);
	
		[Export ("initialFirstResponder")]
		NSView InitialFirstResponder { get; set; }
	
		[Export ("selectNextKeyView:")]
		void SelectNextKeyView (NSObject sender);
	
		[Export ("selectPreviousKeyView:")]
		void SelectPreviousKeyView (NSObject sender);
	
		[Export ("selectKeyViewFollowingView:")]
		void SelectKeyViewFollowingView (NSView aView);
	
		[Export ("selectKeyViewPrecedingView:")]
		void SelectKeyViewPrecedingView (NSView aView);
	
		[Export ("keyViewSelectionDirection")]
		NSSelectionDirection KeyViewSelectionDirection ();
	
		[Export ("defaultButtonCell")]
		[NullAllowed]
		NSButtonCell DefaultButtonCell { get; set; }
	
		[Export ("disableKeyEquivalentForDefaultButtonCell")]
		void DisableKeyEquivalentForDefaultButtonCell ();
	
		[Export ("enableKeyEquivalentForDefaultButtonCell")]
		void EnableKeyEquivalentForDefaultButtonCell ();
	
		[Export ("autorecalculatesKeyViewLoop")]
		bool AutorecalculatesKeyViewLoop  { get; set; }
	
		[Export ("recalculateKeyViewLoop")]
		void RecalculateKeyViewLoop ();
	
		[Export ("toolbar")]
		NSToolbar Toolbar { get; set; }
	
		[Export ("toggleToolbarShown:")]
		void ToggleToolbarShown (NSObject sender);
	
		[Export ("runToolbarCustomizationPalette:")]
		void RunToolbarCustomizationPalette (NSObject sender);
	
		[Export ("showsToolbarButton")]
		bool ShowsToolbarButton { get; set; }

		[Export ("registerForDraggedTypes:")]
		void RegisterForDraggedTypes (string [] newTypes);
	
		[Export ("unregisterDraggedTypes")]
		void UnregisterDraggedTypes ();
	
		[Export ("windowRef")]
		IntPtr WindowRef { get; }

		[Lion, Export ("disableSnapshotRestoration")]
		void DisableSnapshotRestoration ();

		[Lion, Export ("enableSnapshotRestoration")]
		void EnableSnapshotRestoration ();

		//Detected properties
		[Lion, Export ("restorable")]
		bool Restorable { [Bind ("isRestorable")]get; set; }

		[Lion, Export ("restorationClass")]
		Class RestorationClass { get; set; }

		[Lion, Export ("updateConstraintsIfNeeded")]
		void UpdateConstraintsIfNeeded ();

		[Lion, Export ("layoutIfNeeded")]
		void LayoutIfNeeded ();

		[Lion, Export ("setAnchorAttribute:forOrientation:")]
		void SetAnchorAttribute (NSLayoutAttribute layoutAttribute, NSLayoutConstraintOrientation forOrientation);

		[Lion, Export ("visualizeConstraints:")]
		void VisualizeConstraints (NSLayoutConstraint [] constraints);

                [Lion, Export ("convertRectToScreen:")]
		CGRect ConvertRectToScreen (CGRect aRect);

                [Lion, Export ("convertRectFromScreen:")]
		CGRect ConvertRectFromScreen (CGRect aRect);

                [Lion, Export ("convertRectToBacking:")]
		CGRect ConvertRectToBacking (CGRect aRect);

                [Lion, Export ("convertRectFromBacking:")]
		CGRect ConvertRectFromBacking (CGRect aRect);

                [Lion, Export ("backingAlignedRect:options:")]
		CGRect BackingAlignedRect (CGRect aRect, NSAlignmentOptions options);

                [Lion, Export ("backingScaleFactor")]
                nfloat BackingScaleFactor { get; }

                [Lion, Export ("toggleFullScreen:")]
                void ToggleFullScreen ([NullAllowed] NSObject sender);

                //Detected properties
                [Export ("animationBehavior")]
                NSWindowAnimationBehavior AnimationBehavior { get; set; }

#if !XAMARIN_MAC
		//
		// Fields
		//
		[Field ("NSWindowDidBecomeKeyNotification")]
		NSString DidBecomeKeyNotification { get; }

		[Field ("NSWindowDidBecomeMainNotification")]
		NSString DidBecomeMainNotification { get; }

		[Field ("NSWindowDidChangeScreenNotification")]
		NSString DidChangeScreenNotification { get; }

		[Field ("NSWindowDidDeminiaturizeNotification")]
		NSString DidDeminiaturizeNotification { get; }

		[Field ("NSWindowDidExposeNotification")]
		NSString DidExposeNotification { get; }

		[Field ("NSWindowDidMiniaturizeNotification")]
		NSString DidMiniaturizeNotification { get; }

		[Field ("NSWindowDidMoveNotification")]
		NSString DidMoveNotification { get; }

		[Field ("NSWindowDidResignKeyNotification")]
		NSString DidResignKeyNotification { get; }

		[Field ("NSWindowDidResignMainNotification")]
		NSString DidResignMainNotification { get; }

		[Field ("NSWindowDidResizeNotification")]
		NSString DidResizeNotification { get; }

		[Field ("NSWindowDidUpdateNotification")]
		NSString DidUpdateNotification { get; }

		[Field ("NSWindowWillCloseNotification")]
		NSString WillCloseNotification { get; }

		[Field ("NSWindowWillMiniaturizeNotification")]
		NSString WillMiniaturizeNotification { get; }

		[Field ("NSWindowWillMoveNotification")]
		NSString WillMoveNotification { get; }

		[Field ("NSWindowWillBeginSheetNotification")]
		NSString WillBeginSheetNotification { get; }

		[Field ("NSWindowDidEndSheetNotification")]
		NSString DidEndSheetNotification { get; }

		[Field ("NSWindowDidChangeScreenProfileNotification")]
		NSString DidChangeScreenProfileNotification { get; }

		[Field ("NSWindowWillStartLiveResizeNotification")]
		NSString WillStartLiveResizeNotification { get; }

		[Field ("NSWindowDidEndLiveResizeNotification")]
		NSString DidEndLiveResizeNotification { get; }

		[Field ("NSWindowWillEnterFullScreenNotification")]
		NSString WillEnterFullScreenNotification { get; }

		[Mac (10, 7), Field ("NSWindowDidEnterFullScreenNotification")]
		[Notification]
		NSString DidEnterFullScreenNotification { get; }

		[Mac (10, 7), Field ("NSWindowWillExitFullScreenNotification")]
		[Notification]
		NSString WillExitFullScreenNotification { get; }

		[Mac (10, 7), Field ("NSWindowDidExitFullScreenNotification")]
		[Notification]
		NSString DidExitFullScreenNotification { get; }

		[Mac (10, 7), Field ("NSWindowWillEnterVersionBrowserNotification")]
		[Notification]
		NSString WillEnterVersionBrowserNotification { get; }

		[Mac (10, 7), Field ("NSWindowDidEnterVersionBrowserNotification")]
		[Notification]
		NSString DidEnterVersionBrowserNotification { get; }

		[Mac (10, 7), Field ("NSWindowWillExitVersionBrowserNotification")]
		[Notification]
		NSString WillExitVersionBrowserNotification { get; }

		[Mac (10, 7), Field ("NSWindowDidExitVersionBrowserNotification")]
		[Notification]
		NSString DidExitVersionBrowserNotification { get; }

		[Mac (10, 7), Field ("NSWindowDidChangeBackingPropertiesNotification")]
		[Notification (typeof (NSWindowBackingPropertiesEventArgs))]
		NSString DidChangeBackingPropertiesNotification { get; }
		
		// 10.10
		[Mac (10,10)]
		[Export ("titleVisibility")]
		NSWindowTitleVisibility TitleVisibility { get; set; }

		[Mac (10,10)]
		[Export ("titlebarAppearsTransparent")]
		bool TitlebarAppearsTransparent { get; set; }

		[Mac (10,10)]
		[Export ("contentLayoutRect")]
		CGRect ContentLayoutRect { get; }

		[Mac (10,10)]
		[Export ("contentLayoutGuide")]
		NSObject ContentLayoutGuide { get; }

		[Mac (10,10)]
		[Export ("titlebarAccessoryViewControllers", ArgumentSemantic.Copy)]
		// Header says this is a r/w property, but it fails at runtime.
		//  -[NSWindow setTitlebarAccessoryViewControllers:]: unrecognized selector sent to instance 0x6180001e0f00
		NSTitlebarAccessoryViewController [] TitlebarAccessoryViewControllers { get; }

		[Mac (10,10)]
		[Export ("addTitlebarAccessoryViewController:")]
		void AddTitlebarAccessoryViewController (NSTitlebarAccessoryViewController childViewController);

		[Mac (10,10)]
		[Export ("insertTitlebarAccessoryViewController:atIndex:")]
		void InsertTitlebarAccessoryViewController (NSTitlebarAccessoryViewController childViewController, nint index);

		[Mac (10,10)]
		[Export ("removeTitlebarAccessoryViewControllerAtIndex:")]
		void RemoveTitlebarAccessoryViewControllerAtIndex (nint index);

		[Mac (10,10)]
		[Static, Export ("windowWithContentViewController:")]
		NSWindow GetWindowWithContentViewController (NSViewController contentViewController);

		[Mac (10,10)]
		[Export ("contentViewController", ArgumentSemantic.Strong)]
		NSViewController ContentViewController { get; set; }

		[Mac (10,10)]
		[Export ("trackEventsMatchingMask:timeout:mode:handler:")]
		void TrackEventsMatching (NSEventMask mask, double timeout, string mode, NSWindowTrackEventsMatchingCompletionHandler trackingHandler);

		[Mac (10,9)]
		[Export ("sheets", ArgumentSemantic.Copy)]
		NSWindow [] Sheets { get; }

		[Mac (10,9)]
		[Export ("sheetParent", ArgumentSemantic.Retain)]
		NSWindow SheetParent { get; }

		[Mac (10,9)]
		[Export ("occlusionState")]
		NSWindowOcclusionState OcclusionState { get; }

		[Mac (10,9)]
		[Export ("beginSheet:completionHandler:")]
		void BeginSheet (NSWindow sheetWindow, Action<nint> completionHandler);

		[Mac (10,9)]
		[Export ("beginCriticalSheet:completionHandler:")]
		void BeginCriticalSheet (NSWindow sheetWindow, Action<nint> completionHandler);

		[Mac (10,9)]
		[Export ("endSheet:")]
		void EndSheet (NSWindow sheetWindow);

		[Mac (10,9)]
		[Export ("endSheet:returnCode:")]
		void EndSheet (NSWindow sheetWindow, NSModalResponse returnCode);
		
		[Mac (10,11)]
		[Export ("minFullScreenContentSize", ArgumentSemantic.Assign)]
		CGSize MinFullScreenContentSize { get; set; }

		[Mac (10,11)]
		[Export ("maxFullScreenContentSize", ArgumentSemantic.Assign)]
		CGSize MaxFullScreenContentSize { get; set; }

		[Mac (10,11)]
		[Export ("performWindowDragWithEvent:")]
		void PerformWindowDrag(NSEvent theEvent);

		[Mac (10,12)]
		[Export ("canRepresentDisplayGamut:")]
		bool CanRepresentDisplayGamut (NSDisplayGamut displayGamut);

		[Mac (10,12)]
		[Export ("convertPointToScreen:")]
		CGPoint ConvertPointToScreen (CGPoint point);

		[Mac (10,12)]
		[Export ("convertPointFromScreen:")]
		CGPoint ConvertPointFromScreen (CGPoint point);

		[Mac (10,14)]
		[Export ("convertPointToBacking:")]
		CGPoint ConvertPointToBacking (CGPoint point);

		[Mac (10,14)]
		[Export ("convertPointFromBacking:")]
		CGPoint ConvertPointFromBacking (CGPoint point);

		[Mac (10, 14)]
		[NullAllowed]
		[Export ("appearanceSource", ArgumentSemantic.Weak)]
		NSAppearanceCustomization AppearanceSource { get; set; }

		[Mac (11, 0)]
		[Export ("subtitle")]
		string Subtitle { get; set; }

		[Mac (11, 0)]
		[Export ("toolbarStyle", ArgumentSemantic.Assign)]
		NSWindowToolbarStyle ToolbarStyle { get; set; }

		[Mac (11, 0)]
		[Export ("titlebarSeparatorStyle", ArgumentSemantic.Assign)]
		NSTitlebarSeparatorStyle TitlebarSeparatorStyle { get; set; }
		

		[Mac (10, 12)]
		[Static]
		[Export ("allowsAutomaticWindowTabbing")]
		bool AllowsAutomaticWindowTabbing { get; set; }

		[Mac (10, 12)]
		[Static]
		[Export ("userTabbingPreference")]
		NSWindowUserTabbingPreference UserTabbingPreference { get; }

		[Mac (10, 12)]
		[Export ("tabbingMode", ArgumentSemantic.Assign)]
		NSWindowTabbingMode TabbingMode { get; set; }

		[Mac (10, 12)]
		[Export ("tabbingIdentifier")]
		string TabbingIdentifier { get; set; }

		[Mac (10,12)]
		[Export ("selectNextTab:")]
		void SelectNextTab ([NullAllowed] NSObject sender);

		[Mac (10,12)]
		[Export ("selectPreviousTab:")]
		void SelectPreviousTab ([NullAllowed] NSObject sender);

		[Mac (10,12)]
		[Export ("moveTabToNewWindow:")]
		void MoveTabToNewWindow ([NullAllowed] NSObject sender);

		[Mac (10,12)]
		[Export ("mergeAllWindows:")]
		void MergeAllWindows ([NullAllowed] NSObject sender);

		[Mac (10,12)]
		[Export ("toggleTabBar:")]
		void ToggleTabBar ([NullAllowed] NSObject sender);

		[Mac (10, 12)]
		[NullAllowed, Export ("tabbedWindows", ArgumentSemantic.Copy)]
		NSWindow[] TabbedWindows { get; }

		[Mac (10,12)]
		[Export ("addTabbedWindow:ordered:")]
		void AddTabbedWindow (NSWindow window, NSWindowOrderingMode ordered);

		[Mac (10, 12)]
		[Export ("windowTitlebarLayoutDirection")]
		NSUserInterfaceLayoutDirection WindowTitlebarLayoutDirection { get; }

#endif

		// [Mavericks]
		// [Export ("effectiveAppearance")]
		// NSAppearance EffectiveAppearance { get; }
		
		// [Mavericks]
		// [Export ("appearance")]
		// NSAppearance Appearance { get; [Export ("setAppearance:")] set; }
	}

	[Mac (10,10)]
	[BaseType (typeof (NSViewController))]
	public interface NSTitlebarAccessoryViewController : NSAnimationDelegate, NSAnimatablePropertyContainer {
		[Export ("initWithNibName:bundle:")]
		IntPtr Constructor ([NullAllowed] string nibNameOrNull, [NullAllowed] NSBundle nibBundleOrNull);

		[Export ("layoutAttribute")]
		NSLayoutAttribute LayoutAttribute { get; set; }

		[Export ("fullScreenMinHeight")]
		nfloat FullScreenMinHeight { get; set; }

		// [RequiresSuper]
		[Export ("viewWillAppear")]
		void ViewWillAppear ();

		// [RequiresSuper]
		[Export ("viewDidAppear")]
		void ViewDidAppear ();

		// [RequiresSuper]
		[Export ("viewDidDisappear")]
		void ViewDidDisappear ();

		[Mac (10,12)]
		[Export ("hidden")]
		bool IsHidden { [Bind ("isHidden")] get; set; }

		[Mac (11,0)]
		[Export ("automaticallyAdjustsSize")]
		bool AutomaticallyAdjustsSize { get; set; }
	}
	
	[Mac (10,10)]
	// [NoMacCatalyst]
	[BaseType (typeof (NSView))]
	interface NSVisualEffectView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		[Export ("material")]
		NSVisualEffectMaterial Material { get; set; }

		[Export ("interiorBackgroundStyle")]
		NSBackgroundStyle InteriorBackgroundStyle { get; }

		[Export ("blendingMode")]
		NSVisualEffectBlendingMode BlendingMode { get; set; }

		[Export ("state")]
		NSVisualEffectState State { get; set; }

		[Export ("maskImage", ArgumentSemantic.Retain)]
		NSImage MaskImage { get; set; }

		// [RequiresSuper]
		[Export ("viewDidMoveToWindow")]
		void ViewDidMove ();

		// [RequiresSuper]
		[Export ("viewWillMoveToWindow:")]
		void ViewWillMove (NSWindow newWindow);

		[Mac (10, 12)]
		[Export ("emphasized")]
		bool Emphasized { [Bind ("isEmphasized")] get; set; }
	}

	public delegate void NSWindowCompletionHandler (NSWindow window, NSError error);

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	[Model]
	[Lion]
	public partial interface NSWindowRestoration {
		[Static]
		[Export ("restoreWindowWithIdentifier:state:completionHandler:")]
		void RestoreWindow (string identifier, NSCoder state, NSWindowCompletionHandler onCompletion);

	}

	//64 bit reviewed
	[BaseType (typeof (NSResponder))]
	public interface NSWindowController {
		[Export ("initWithWindow:")]
		IntPtr Constructor (NSWindow  window);
	
		[Export ("initWithWindowNibName:")]
		IntPtr Constructor (string  windowNibName);
	
		[Export ("initWithWindowNibName:owner:")]
		IntPtr Constructor (string  windowNibName, NSObject owner);
	
		[Export ("windowNibName")]
		string WindowNibName { get; }
	
		[Export ("windowNibPath")]
		string WindowNibPath { get; }
	
		[Export ("owner")]
		NSObject Owner { get; }
	
		[Export ("windowFrameAutosaveName")]
		string WindowFrameAutosaveName { get; set; }
	
		[Export ("shouldCascadeWindows")]
		bool ShouldCascadeWindows  { get; set; }
	
		[Export ("document")]
		NSDocument Document { get; set; }
	
		[Export ("setDocumentEdited:")]
		void SetDocumentEdited (bool dirtyFlag);
	
		[Export ("shouldCloseDocument")]
		bool ShouldCloseDocument  { get; set; }
	
		[Export ("window")]
		NSWindow Window { get; set; }
	
		[Export ("synchronizeWindowTitleWithDocumentName")]
		void SynchronizeWindowTitleWithDocumentName ();
	
		[Export ("windowTitleForDocumentDisplayName:")]
		string WindowTitleForDocumentDisplayName (string  displayName);
	
		[Export ("close")]
		void Close ();
	
		[Export ("showWindow:")]
		void ShowWindow (NSObject sender);
	
		[Export ("isWindowLoaded")]
		bool IsWindowLoaded  { get; }
	
		[Export ("windowWillLoad")]
		void WindowWillLoad ();
	
		[Export ("windowDidLoad")]
		void WindowDidLoad ();
	
		[Export ("loadWindow")]
		void LoadWindow ();
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSWindowDelegate {
		[Export ("windowShouldClose:"), DelegateName ("NSObjectPredicate"), DefaultValue (true)]
		bool WindowShouldClose (NSObject sender);
	
		[Export ("windowWillReturnFieldEditor:toObject:"), DelegateName ("NSWindowClient"), DefaultValue (null)]
		NSObject WillReturnFieldEditor (NSWindow  sender, NSObject client);
	
		[Export ("windowWillResize:toSize:"), DelegateName ("NSWindowResize"), DefaultValueFromArgument ("toFrameSize")]
		CGSize WillResize (NSWindow sender, CGSize toFrameSize);
	
		[Export ("windowWillUseStandardFrame:defaultFrame:"), DelegateName ("NSWindowFrame"), DefaultValueFromArgument ("newFrame")]
		CGRect WillUseStandardFrame (NSWindow window, CGRect newFrame);
	
		[Export ("windowShouldZoom:toFrame:"), DelegateName ("NSWindowFramePredicate"), DefaultValue (true)]
		bool ShouldZoom (NSWindow  window, CGRect newFrame);

		[Export ("windowWillReturnUndoManager:"), DelegateName ("NSWindowUndoManager"), DefaultValue (null)]
		NSUndoManager WillReturnUndoManager (NSWindow  window);
	
		[Export ("window:willPositionSheet:usingRect:"), DelegateName ("NSWindowSheetRect"), DefaultValueFromArgument ("usingRect")]
		CGRect WillPositionSheet (NSWindow  window, NSWindow  sheet, CGRect usingRect);

		[Export ("window:shouldPopUpDocumentPathMenu:"), DelegateName ("NSWindowMenu"), DefaultValue (true)]
		bool ShouldPopUpDocumentPathMenu (NSWindow  window, NSMenu  menu);
	
		[Export ("window:shouldDragDocumentWithEvent:from:withPasteboard:"), DelegateName ("NSWindowDocumentDrag"), DefaultValue (true)]
		bool ShouldDragDocumentWithEvent (NSWindow  window, NSEvent theEvent, CGPoint dragImageLocation, NSPasteboard  withPasteboard);

		[Export ("windowDidResize:"), EventArgs ("NSNotification")]
		void DidResize (NSNotification  notification);
	
		[Export ("windowDidExpose:"), EventArgs ("NSNotification")]
		void DidExpose (NSNotification  notification);
	
		[Export ("windowWillMove:"), EventArgs ("NSNotification")]
		void WillMove (NSNotification  notification);
	
		[Export ("windowDidMove:"), EventArgs ("NSNotification")]
		void DidMoved (NSNotification  notification);
	
		[Export ("windowDidBecomeKey:"), EventArgs ("NSNotification")]
		void DidBecomeKey (NSNotification  notification);
	
		[Export ("windowDidResignKey:"), EventArgs ("NSNotification")]
		void DidResignKey (NSNotification  notification);
	
		[Export ("windowDidBecomeMain:"), EventArgs ("NSNotification")]
		void DidBecomeMain (NSNotification  notification);
	
		[Export ("windowDidResignMain:"), EventArgs ("NSNotification")]
		void DidResignMain (NSNotification  notification);
	
		[Export ("windowWillClose:"), EventArgs ("NSNotification")]
		void WillClose (NSNotification  notification);
	
		[Export ("windowWillMiniaturize:"), EventArgs ("NSNotification")]
		void WillMiniaturize (NSNotification  notification);
	
		[Export ("windowDidMiniaturize:"), EventArgs ("NSNotification")]
		void DidMiniaturize (NSNotification  notification);
	
		[Export ("windowDidDeminiaturize:"), EventArgs ("NSNotification")]
		void DidDeminiaturize (NSNotification  notification);
	
		[Export ("windowDidUpdate:"), EventArgs ("NSNotification")]
		void DidUpdate (NSNotification  notification);
	
		[Export ("windowDidChangeScreen:"), EventArgs ("NSNotification")]
		void DidChangeScreen (NSNotification  notification);
	
		[Export ("windowDidChangeScreenProfile:"), EventArgs ("NSNotification")]
		void DidChangeScreenProfile (NSNotification notification);
	
		[Export ("windowWillBeginSheet:"), EventArgs ("NSNotification")]
		void WillBeginSheet (NSNotification notification);
	
		[Export ("windowDidEndSheet:"), EventArgs ("NSNotification")]
		void DidEndSheet (NSNotification notification);
	
		[Export ("windowWillStartLiveResize:"), EventArgs ("NSNotification")]
		void WillStartLiveResize (NSNotification notification);
	
		[Export ("windowDidEndLiveResize:"), EventArgs ("NSNotification")]
		void DidEndLiveResize (NSNotification notification);

		[Lion, Export ("windowWillEnterFullScreen:"), EventArgs ("NSNotification")]
		void WillEnterFullScreen (NSNotification notification);

		[Lion, Export ("windowDidEnterFullScreen:"), EventArgs ("NSNotification")]
		void DidEnterFullScreen (NSNotification notification);

		[Lion, Export ("windowWillExitFullScreen:"), EventArgs ("NSNotification")]
		void WillExitFullScreen (NSNotification  notification);
		
		[Lion, Export ("windowDidExitFullScreen:"), EventArgs ("NSNotification")]
		void DidExitFullScreen (NSNotification notification);

		[Lion, Export ("windowDidFailToEnterFullScreen:"), EventArgs ("NSWindow")]
		void DidFailToEnterFullScreen (NSWindow window);

		[Lion, Export ("windowDidFailToExitFullScreen:"), EventArgs ("NSWindow")]
		void DidFailToExitFullScreen (NSWindow window);
		
		[Lion, Export ("window:willUseFullScreenContentSize:"), DelegateName ("NSWindowSize"), DefaultValueFromArgument ("proposedSize")]
		CGSize WillUseFullScreenContentSize (NSWindow  window, CGSize proposedSize);

		[Lion, Export ("window:willUseFullScreenPresentationOptions:"), DelegateName ("NSWindowApplicationPresentationOptions"), DefaultValueFromArgument ("proposedOptions")]
		NSApplicationPresentationOptions WillUseFullScreenPresentationOptions (NSWindow  window, NSApplicationPresentationOptions proposedOptions);
		
		[Lion, Export ("customWindowsToEnterFullScreenForWindow:"), DelegateName ("NSWindowWindows"), DefaultValue (null)]
		NSWindow[] CustomWindowsToEnterFullScreen (NSWindow  window);

		[Lion, Export ("customWindowsToExitFullScreenForWindow:"), DelegateName ("NSWindowWindows"), DefaultValue (null)]
		NSWindow[] CustomWindowsToExitFullScreen (NSWindow  window);

		[Lion, Export ("window:startCustomAnimationToEnterFullScreenWithDuration:"), EventArgs("NSWindowDuration")]
		void StartCustomAnimationToEnterFullScreen (NSWindow  window, double duration);

		[Lion, Export ("window:startCustomAnimationToExitFullScreenWithDuration:"), EventArgs("NSWindowDuration")]
		void StartCustomAnimationToExitFullScreen (NSWindow  window, double duration);

		[Lion, Export ("window:willEncodeRestorableState:"), EventArgs ("NSWindowCoder")]
		void WillEncodeRestorableState(NSWindow window, NSCoder coder);
		
		[Lion, Export ("window:didDecodeRestorableState:"), EventArgs ("NSWindowCoder")]
		void DidDecodeRestorableState(NSWindow window, NSCoder coder);
		
		[Lion, Export ("window:willResizeForVersionBrowserWithMaxPreferredSize:maxAllowedSize:"), DelegateName ("NSWindowSizeSize"), DefaultValueFromArgument ("maxPreferredSize")]
		CGSize WillResizeForVersionBrowser(NSWindow window, CGSize maxPreferredSize, CGSize maxAllowedSize);

		[Lion, Export ("windowWillEnterVersionBrowser:"), EventArgs ("NSNotification")]
		void WillEnterVersionBrowser (NSNotification notification);
		
		[Lion, Export ("windowDidEnterVersionBrowser:"), EventArgs ("NSNotification")]
		void DidEnterVersionBrowser (NSNotification notification);
		
		[Lion, Export ("windowWillExitVersionBrowser:"), EventArgs ("NSNotification")]
		void WillExitVersionBrowser (NSNotification notification);
		
		[Lion, Export ("windowDidExitVersionBrowser:"), EventArgs ("NSNotification")]
		void DidExitVersionBrowser (NSNotification notification);
		
		[Mac (10, 7), Export ("windowDidChangeBackingProperties:"), EventArgs ("NSNotification")]
		void DidChangeBackingProperties (NSNotification notification);
	}

	interface NSWorkspaceRenamedEventArgs {
		[Export ("NSWorkspaceVolumeLocalizedNameKey")]
		string VolumeLocalizedName { get; }
		
		[Export ("NSWorkspaceVolumeURLKey")]
		NSUrl VolumeUrl { get; }

		[Export ("NSWorkspaceVolumeOldLocalizedNameKey")]
		string OldVolumeLocalizedName { get; }

		[Export ("NSWorkspaceVolumeOldURLKey")]
		NSUrl OldVolumeUrl { get; }
	}

	interface NSWorkspaceMountEventArgs {
		[Export ("NSWorkspaceVolumeLocalizedNameKey")]
		string VolumeLocalizedName { get; }
		
		[Export ("NSWorkspaceVolumeURLKey")]
		NSUrl VolumeUrl { get; }
	}

	interface NSWorkspaceApplicationEventArgs {
		[Export ("NSWorkspaceApplicationKey")]
		NSRunningApplication Application { get; }
	}

	interface NSWorkspaceFileOperationEventArgs {
		[Export ("NSOperationNumber")]
		int FileType { get; }
	}
	
	public delegate void NSWorkspaceUrlHandler (NSDictionary newUrls, NSError error);

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	public interface NSWorkspace {
		[Static]
		[Export ("sharedWorkspace"), ThreadSafe]
		NSWorkspace SharedWorkspace { get; }
		
		[Export ("notificationCenter")]
		NSNotificationCenter NotificationCenter { get; }
		
		[Export ("openFile:")]
		bool OpenFile (string fullPath);
		
		[Export ("openFile:withApplication:")]
		bool OpenFile (string fullPath, string appName);
		
		[Export ("openFile:withApplication:andDeactivate:")]
		bool OpenFile (string fullPath, string appName, bool deactivate);
		
		[Export ("openFile:fromImage:at:inView:")]
		bool OpenFile (string fullPath, NSImage anImage, CGPoint point, NSView aView);

		[Export ("openURL:")]
		bool OpenUrl (NSUrl url);
		
		[Export ("launchApplication:")]
		bool LaunchApplication (string appName);
		
		[Export ("launchApplicationAtURL:options:configuration:error:")]
		NSRunningApplication LaunchApplication (NSUrl url, NSWorkspaceLaunchOptions options, NSDictionary configuration, out NSError error );
		
		[Export ("launchApplication:showIcon:autolaunch:")]
		bool LaunchApplication (string appName, bool showIcon, bool autolaunch);
		
		[Export ("fullPathForApplication:")]
		string FullPathForApplication (string appName);
		
		[Export ("selectFile:inFileViewerRootedAtPath:")]
		bool SelectFile (string fullPath, string rootFullPath);
		
		[Export ("activateFileViewerSelectingURLs:")]
		void ActivateFileViewer (NSUrl[] fileUrls);
		
		[Export ("showSearchResultsForQueryString:")]
		bool ShowSearchResults (string queryString );
		
		[Export ("noteFileSystemChanged:")]
		void NoteFileSystemChanged (string path);
		
		[Export ("getInfoForFile:application:type:")]
		bool GetInfo (string fullPath, out string appName, out string fileType);
		
		[Export ("isFilePackageAtPath:")]
		bool IsFilePackage (string fullPath);
		
		[Export ("iconForFile:")]
		NSImage IconForFile (string fullPath);
		
		[Export ("iconForFiles:")]
		NSImage IconForFiles (string[] fullPaths);
		
		[Export ("iconForFileType:")]
		NSImage IconForFileType (string fileType);
		
		[Export ("setIcon:forFile:options:")]
		bool SetIconforFile (NSImage image, string fullPath, NSWorkspaceIconCreationOptions options);
		
		[Export ("fileLabels")]
		string[] FileLabels { get ; }
		
		[Export ("fileLabelColors")]
		NSColor[] FileLabelColors { get; }
		
		[Export ("recycleURLs:completionHandler:")]
		void RecycleUrls (NSArray urls, NSWorkspaceUrlHandler completionHandler);
		
		[Export ("duplicateURLs:completionHandler:")]
		void DuplicateUrls (NSArray urls, NSWorkspaceUrlHandler completionHandler);
		
		[Export ("getFileSystemInfoForPath:isRemovable:isWritable:isUnmountable:description:type:")]
		bool GetFileSystemInfo (string fullPath, out bool removableFlag, out bool writableFlag, out bool unmountableFlag, out string description, out string fileSystemType);
		
		[Export ("performFileOperation:source:destination:files:tag:")]
		bool PerformFileOperation (NSString workspaceOperation, string source, string destination, string[] files, out nint tag);

		[Export ("unmountAndEjectDeviceAtPath:")]
		bool UnmountAndEjectDevice(string path);

		[Export ("unmountAndEjectDeviceAtURL:error:")]
		bool UnmountAndEjectDevice (NSUrl url, out NSError error);
		
		[Export ("extendPowerOffBy:")]
		nint ExtendPowerOffBy (nint requested);

		[Export ("hideOtherApplications")]
		void HideOtherApplications ();
		
		[Export ("mountedLocalVolumePaths")]
		string[] MountedLocalVolumePaths { get; }
		
		[Export ("mountedRemovableMedia")]
		string[] MountedRemovableMedia {  get; }
		
		[Export ("URLForApplicationWithBundleIdentifier:")]
		NSUrl UrlForApplication (string bundleIdentifier );
		
		[Export ("URLForApplicationToOpenURL:")]
		NSUrl UrlForApplication (NSUrl url );
		
		[Export ("absolutePathForAppBundleWithIdentifier:")]
		string AbsolutePathForAppBundle (string bundleIdentifier);
		
		[Export ("launchAppWithBundleIdentifier:options:additionalEventParamDescriptor:launchIdentifier:")]
		bool LaunchApp (string bundleIdentifier, NSWorkspaceLaunchOptions options, NSAppleEventDescriptor descriptor, IntPtr identifier);
		
		[Export ("openURLs:withAppBundleIdentifier:options:additionalEventParamDescriptor:launchIdentifiers:")]
		bool OpenUrls (NSUrl[] urls, string bundleIdentifier, NSWorkspaceLaunchOptions options, NSAppleEventDescriptor descriptor, string[] identifiers);
		
		[Export ("launchedApplications")]
		NSDictionary [] LaunchedApplications { get; }
		
		[Export ("activeApplication")]
		NSDictionary ActiveApplication { get; }
		
		[Export ("typeOfFile:error:")]
		string TypeOfFile (string absoluteFilePath, out NSError outError);
		
		[Export ("localizedDescriptionForType:")]
		string LocalizedDescription (string typeName);
		
		[Export ("preferredFilenameExtensionForType:")]
		string PreferredFilenameExtension (string typeName);
		
		[Export ("filenameExtension:isValidForType:")]
		bool IsFilenameExtensionValid (string filenameExtension, string typeName);
		
		[Export ("type:conformsToType:")]
		bool TypeConformsTo (string firstTypeName, string secondTypeName);
		
		[Export ("setDesktopImageURL:forScreen:options:error:")]
		bool SetDesktopImageUrl (NSUrl url, NSScreen screen, NSDictionary options, NSError error );
		
		[Export ("desktopImageURLForScreen:")]
		NSUrl DesktopImageUrl (NSScreen screen );
		
		[Export ("desktopImageOptionsForScreen:")]
		NSDictionary DesktopImageOptions (NSScreen screen);		
		
		[Export ("runningApplications"), ThreadSafe]
		NSRunningApplication [] RunningApplications { get; }

		[Lion]
		[Export ("frontmostApplication")]
		NSRunningApplication FrontmostApplication { get; }

		[Lion]
		[Export ("menuBarOwningApplication")]
		NSRunningApplication MenuBarOwningApplication { get; }

		[Field ("NSWorkspaceWillPowerOffNotification")]
		[Notification ("SharedWorkspace.NotificationCenter")]
		NSString WillPowerOffNotification { get; }

		[Field ("NSWorkspaceWillSleepNotification")]
		[Notification ("SharedWorkspace.NotificationCenter")]
		NSString WillSleepNotification { get; }
		
		[Field ("NSWorkspaceDidWakeNotification")]
		[Notification ("SharedWorkspace.NotificationCenter")]
		NSString DidWakeNotification { get; }
		
		[Field ("NSWorkspaceScreensDidSleepNotification")]
		[Notification ("SharedWorkspace.NotificationCenter")]
		NSString ScreensDidSleepNotification { get; }
		
		[Field ("NSWorkspaceScreensDidWakeNotification")]
		[Notification ("SharedWorkspace.NotificationCenter")]
		NSString ScreensDidWakeNotification { get; }
		
		[Field ("NSWorkspaceSessionDidBecomeActiveNotification")]
		[Notification ("SharedWorkspace.NotificationCenter")]
		NSString SessionDidBecomeActiveNotification { get; }
		
		[Field ("NSWorkspaceSessionDidResignActiveNotification")]
		[Notification ("SharedWorkspace.NotificationCenter")]
		NSString SessionDidResignActiveNotification { get; }

		[Field ("NSWorkspaceDidRenameVolumeNotification")]
		[Notification (typeof (NSWorkspaceRenamedEventArgs), "SharedWorkspace.NotificationCenter")]
		NSString DidRenameVolumeNotification { get; }

		[Field ("NSWorkspaceDidMountNotification")]
		[Notification (typeof (NSWorkspaceMountEventArgs), "SharedWorkspace.NotificationCenter")]
		NSString DidMountNotification { get; }
		
		[Field ("NSWorkspaceDidUnmountNotification")]
		[Notification (typeof (NSWorkspaceMountEventArgs), "SharedWorkspace.NotificationCenter")]
		NSString DidUnmountNotification { get; }
		
		[Field ("NSWorkspaceWillUnmountNotification")]
		[Notification (typeof (NSWorkspaceMountEventArgs), "SharedWorkspace.NotificationCenter")]
		NSString WillUnmountNotification { get; }

		[Field ("NSWorkspaceWillLaunchApplicationNotification")]
		[Notification (typeof (NSWorkspaceApplicationEventArgs), "SharedWorkspace.NotificationCenter")]
		NSString WillLaunchApplication { get; }

		[Field ("NSWorkspaceDidLaunchApplicationNotification")]
		[Notification (typeof (NSWorkspaceApplicationEventArgs), "SharedWorkspace.NotificationCenter")]
		NSString DidLaunchApplicationNotification { get; }
		
		[Field ("NSWorkspaceDidTerminateApplicationNotification")]
		[Notification (typeof (NSWorkspaceApplicationEventArgs), "SharedWorkspace.NotificationCenter")]
		NSString DidTerminateApplicationNotification { get; }
		
		[Field ("NSWorkspaceDidHideApplicationNotification")]
		[Notification (typeof (NSWorkspaceApplicationEventArgs), "SharedWorkspace.NotificationCenter")]
		NSString DidHideApplicationNotification { get; }
		
		[Field ("NSWorkspaceDidUnhideApplicationNotification")]
		[Notification (typeof (NSWorkspaceApplicationEventArgs), "SharedWorkspace.NotificationCenter")]
		NSString DidUnhideApplicationNotification { get; }
		
		[Field ("NSWorkspaceDidActivateApplicationNotification")]
		[Notification (typeof (NSWorkspaceApplicationEventArgs), "SharedWorkspace.NotificationCenter")]
		NSString DidActivateApplicationNotification { get; }
		
		[Field ("NSWorkspaceDidDeactivateApplicationNotification")]
		[Notification (typeof (NSWorkspaceApplicationEventArgs), "SharedWorkspace.NotificationCenter")]
		NSString DidDeactivateApplicationNotification { get; }
		
		[Field ("NSWorkspaceDidPerformFileOperationNotification")]
		[Notification (typeof (NSWorkspaceFileOperationEventArgs), "SharedWorkspace.NotificationCenter")]
		NSString DidPerformFileOperationNotification { get; }
		
		[Field ("NSWorkspaceDidChangeFileLabelsNotification")]
		[Notification ("SharedWorkspace.NotificationCenter")]
		NSString DidChangeFileLabelsNotification { get; }

		[Field ("NSWorkspaceActiveSpaceDidChangeNotification")]
		[Notification ("SharedWorkspace.NotificationCenter")]
		NSString ActiveSpaceDidChangeNotification { get; }
		
		//
		// File operations
		//
		// Those not listed are not here, because they are documented as returing an error
		//
		[Field ("NSWorkspaceRecycleOperation")]
		NSString OperationRecycle { get; }

		[Field ("NSWorkspaceDuplicateOperation")]
		NSString OperationDuplicate { get; }

		[Field ("NSWorkspaceMoveOperation")]
		NSString OperationMove { get; }
		
		[Field ("NSWorkspaceCopyOperation")]
		NSString OperationCopy { get; }
		
		[Field ("NSWorkspaceLinkOperation")]
		NSString OperationLink { get; }
		
		[Field ("NSWorkspaceDestroyOperation")]
		NSString OperationDestroy { get; }
	}
	
	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	public partial interface NSRunningApplication {
		[Export ("terminated")]
		bool Terminated { [Bind ("isTerminated")] get;  }
		
		[Export ("finishedLaunching")]
		bool FinishedLaunching { [Bind ("isFinishedLaunching")] get;  }
		
		[Export ("hidden")]
		bool Hidden { [Bind ("isHidden")] get;  }
		
		[Export ("active")]
		bool Active { [Bind ("isActive")] get;  }
		
		[Export ("activationPolicy")]
		NSApplicationActivationPolicy ActivationPolicy { get;  }
		
		[Export ("localizedName")]
		string LocalizedName { get;  }
		
		[Export ("bundleIdentifier")]
		string BundleIdentifier { get;  }
		
		[Export ("bundleURL")]
		NSUrl BundleUrl { get;  }
		
		[Export ("executableURL")]
		NSUrl ExecutableUrl { get;  }
		
		// changed pid_t to int
		[Export ("processIdentifier")]
		int ProcessIdentifier { get;  }
		
		[Export ("launchDate")]
		NSDate LaunchDate { get;  }
		
		[Export ("icon")]
		NSImage Icon { get;  }
		
		[Export ("executableArchitecture")]
		int ExecutableArchitecture { get;  }
		
		[Export ("hide")]
		bool Hide ();
		
		[Export ("unhide")]
		bool Unhide ();
		
		[Export ("activateWithOptions:")]
		bool Activate (NSApplicationActivationOptions options);
		
		[Export ("terminate")]
		bool Terminate ();
		
		[Export ("forceTerminate")]
		bool ForceTerminate ();
		
		[Static]
		[Export ("runningApplicationsWithBundleIdentifier:")]
		NSRunningApplication[] GetRunningApplications (string bundleIdentifier);

		[Static]
		[Export ("runningApplicationWithProcessIdentifier:")]
		NSRunningApplication GetRunningApplication (int pid);
		
		[Static][ThreadSafe]
		[Export ("currentApplication")]
		NSRunningApplication CurrentApplication { get ; }
	
	}	

	//64 bit reviewed
	[BaseType (typeof (NSControl))]
	public interface NSStepper {
		[Export ("initWithFrame:")]
		IntPtr Constructor (CGRect frameRect);

		//Detected properties
		[Export ("minValue")]
		double MinValue { get; set; }

		[Export ("maxValue")]
		double MaxValue { get; set; }

		[Export ("increment")]
		double Increment { get; set; }

		[Export ("valueWraps")]
		bool ValueWraps { get; set; }

		[Export ("autorepeat")]
		bool Autorepeat { get; set; }

	}

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	public interface NSPredicateEditorRowTemplate {
	        [Export ("matchForPredicate:")]
		double MatchForPredicate (NSPredicate predicate);

		[Export ("templateViews")]
		NSObject[] TemplateViews { get; }

		[Export ("setPredicate:")]
		void SetPredicate (NSPredicate predicate);

		[Export ("predicateWithSubpredicates:")]
		NSPredicate PredicateWithSubpredicates (NSPredicate[] subpredicates);
		
		[Export ("displayableSubpredicatesOfPredicate:")]
		NSPredicate[] DisplayableSubpredicatesOfPredicate (NSPredicate predicate);

		[Export ("initWithLeftExpressions:rightExpressions:modifier:operators:options:")]
		//NSObject InitWithLeftExpressionsrightExpressionsmodifieroperatorsoptions (NSArray leftExpressions, NSArray rightExpressions, NSComparisonPredicateModifier modifier, NSArray operators, uint options);
		IntPtr Constructor (NSExpression[] leftExpressions, NSExpression[] rightExpressions, NSComparisonPredicateModifier modifier, NSObject[] operators, NSComparisonPredicateOptions options);

		[Export ("initWithLeftExpressions:rightExpressionAttributeType:modifier:operators:options:")]
		//NSObject InitWithLeftExpressionsrightExpressionAttributeTypemodifieroperatorsoptions (NSArray leftExpressions, NSAttributeType attributeType, NSComparisonPredicateModifier modifier, NSArray operators, uint options);
		IntPtr Constructor (NSExpression[] leftExpressions, NSAttributeType attributeType, NSComparisonPredicateModifier modifier, NSObject[] operators, NSComparisonPredicateOptions options);

		[Export ("initWithCompoundTypes:")]
		IntPtr Constructor (NSNumber[] compoundTypes);

		[Export ("leftExpressions")]
		NSExpression[] LeftExpressions { get; }

		[Export ("rightExpressions")]
		NSExpression[] RightExpressions { get; }

		[Export ("rightExpressionAttributeType")]
		NSAttributeType RightExpressionAttributeType { get; }

		[Export ("modifier")]
		NSComparisonPredicateModifier Modifier { get; }

		[Export ("operators")]
		NSObject[] Operators { get; }

		[Export ("options")]
		NSComparisonPredicateOptions Options { get; }

		[Export ("compoundTypes")]
		NSNumber[] CompoundTypes { get; }

		[Static]
		[Export ("templatesWithAttributeKeyPaths:inEntityDescription:")]
		//NSArray TemplatesWithAttributeKeyPathsinEntityDescription (NSArray keyPaths, NSEntityDescription entityDescription);
		NSPredicateEditorRowTemplate[] GetTemplates (string[] keyPaths, NSEntityDescription entityDescription);

	}
   
	//64 bit reviewed
	[BaseType (typeof (NSControl), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSRuleEditorDelegate)})]
	public partial interface NSRuleEditor {
		[Export ("reloadCriteria")]
		void ReloadCriteria ();

		[Export ("predicate")]
		NSPredicate Predicate { get; }

		[Export ("reloadPredicate")]
		void ReloadPredicate ();

		[Export ("predicateForRow:")]
		NSPredicate GetPredicate (nint row);

		[Export ("numberOfRows")]
		nint NumberOfRows { get; }

		[Export ("subrowIndexesForRow:")]
		NSIndexSet SubrowIndexes (nint rowIndex);

		[Export ("criteriaForRow:")]
		NSArray Criteria (nint row);

		[Export ("displayValuesForRow:")]
		NSObject[] DisplayValues (nint row);

		[Export ("rowForDisplayValue:")]
		nint Row (NSObject displayValue);

		[Export ("rowTypeForRow:")]
		NSRuleEditorRowType RowType (nint rowIndex);

		[Export ("parentRowForRow:")]
		nint ParentRow (nint rowIndex);

		[Export ("addRow:")]
		void AddRow (NSObject sender);

		[Export ("insertRowAtIndex:withType:asSubrowOfRow:animate:")]
		void InsertRowAtIndex (nint rowIndex, NSRuleEditorRowType rowType, nint parentRow, bool shouldAnimate);

		[Export ("setCriteria:andDisplayValues:forRowAtIndex:")]
		void SetCriteria (NSArray criteria, NSArray values, nint rowIndex);

		[Export ("removeRowAtIndex:")]
		void RemoveRowAtIndex (nint rowIndex);

		[Export ("removeRowsAtIndexes:includeSubrows:")]
		void RemoveRowsAtIndexes (NSIndexSet rowIndexes, bool includeSubrows);

		[Export ("selectedRowIndexes")]
		NSIndexSet SelectedRows { get; }

		[Export ("selectRowIndexes:byExtendingSelection:")]
		void SelectRows (NSIndexSet indexes, bool extend);

		//Detected properties
		//[Export ("delegate")]
		//NSRuleEditorDelegate Delegate { get; set; }
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSRuleEditorDelegate WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSRuleEditorDelegate Delegate { get; set; }
       
		[Export ("formattingStringsFilename")]
		string FormattingStringsFilename { get; set; }

		[Export ("formattingDictionary")]
		NSDictionary FormattingDictionary { get; set; }

		[Export ("nestingMode")]
		NSRuleEditorNestingMode NestingMode { get; set; }

		[Export ("rowHeight")]
		nfloat RowHeight { get; set; }

		[Export ("editable")]
		bool Editable { [Bind ("isEditable")]get; set; }

		[Export ("canRemoveAllRows")]
		bool CanRemoveAllRows { get; set; }

		[Export ("rowClass")]
		Class RowClass { get; set; }

		[Export ("rowTypeKeyPath")]
		string RowTypeKeyPath { get; set; }

		[Export ("subrowsKeyPath")]
		string SubrowsKeyPath { get; set; }

		[Export ("criteriaKeyPath")]
		string CriteriaKeyPath { get; set; }

		[Export ("displayValuesKeyPath")]
		string DisplayValuesKeyPath { get; set; }
	}

	//64 bit reviewed
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSRuleEditorDelegate {
		[Abstract]
		[Export ("ruleEditor:numberOfChildrenForCriterion:withRowType:"), DelegateName ("NSRuleEditorNumberOfChildren"), DefaultValue(0)]
		nint NumberOfChildren (NSRuleEditor editor, NSObject criterion, NSRuleEditorRowType rowType);

		[Abstract]
		[Export ("ruleEditor:child:forCriterion:withRowType:"), DelegateName ("NSRulerEditorChildCriterion"), DefaultValue(null)]
		NSObject ChildForCriterion (NSRuleEditor editor, nint index, NSObject criterion, NSRuleEditorRowType rowType);

		[Abstract]
		[Export ("ruleEditor:displayValueForCriterion:inRow:"), DelegateName ("NSRulerEditorDisplayValue"), DefaultValue(null)]
		NSObject DisplayValue (NSRuleEditor editor, NSObject criterion, nint row);

		[Abstract]
		[Export ("ruleEditor:predicatePartsForCriterion:withDisplayValue:inRow:"), DelegateName ("NSRulerEditorPredicateParts"), DefaultValue(null)]
		NSDictionary PredicateParts (NSRuleEditor editor, NSObject criterion, NSObject value, nint row);

		[Abstract]
		[Export ("ruleEditorRowsDidChange:"), EventArgs ("NSNotification")]
		void RowsDidChange (NSNotification notification);
		
		[Export ("controlTextDidEndEditing:"), EventArgs ("NSNotification")]
		void EditingEnded (NSNotification notification);

		[Export ("controlTextDidChange:"), EventArgs ("NSNotification")]
		void Changed (NSNotification notification);

		[Export ("controlTextDidBeginEditing:"), EventArgs ("NSNotification")]
		void EditingBegan (NSNotification notification);			

	}
   
	//64 bit reviewed
	[BaseType (typeof (NSRuleEditor))]
	public interface NSPredicateEditor {
		//Detected properties
		[Export ("rowTemplates")]
		NSPredicateEditorRowTemplate[] RowTemplates { get; set; }

	} 	

	// Start of NSSharingService.h
	
	[MountainLion]
	public delegate void NSSharingServiceHandler ();

	//64 bit reviewed
	[MountainLion]
	[BaseType (typeof (NSObject),
	           Delegates=new string [] {"WeakDelegate"},
	Events=new Type [] { typeof (NSSharingServiceDelegate) })]
	public interface NSSharingService 
	{
		
		[Export ("delegate")][NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Wrap ("WeakDelegate")][NullAllowed]
		NSSharingServiceDelegate Delegate { get; set; }
		
		[Export ("title")]
		string Title { get; }
		
		[Export ("image")]
		NSImage Image { get; }
		
		[Export ("alternateImage")]
		NSImage AlternateImage { get; }
		
		[Export ("sharingServicesForItems:")][Static]
		NSSharingService [] SharingServicesForItems (NSObject [] items);
		
		[Export ("sharingServiceNamed:")][Static]
		NSSharingService GetSharingService (NSString serviceName);
		
		[Export ("initWithTitle:image:alternateImage:handler:")]
		IntPtr Constructor (string title, NSImage image, NSImage alternateImage, NSSharingServiceHandler handler);
		
		[Export ("canPerformWithItems:")]
		bool CanPerformWithItems ([NullAllowed] NSObject [] items);

		[Export ("performWithItems:")]
		void PerformWithItems (NSObject [] items);
		
		// Constants

		[Field ("NSSharingServiceNamePostOnFacebook")][Internal]
		NSString NSSharingServiceNamePostOnFacebook { get; }
		
		[Field ("NSSharingServiceNamePostOnTwitter")][Internal]
		NSString NSSharingServiceNamePostOnTwitter { get; }
		
		[Field ("NSSharingServiceNamePostOnSinaWeibo")][Internal]
		NSString NSSharingServiceNamePostOnSinaWeibo { get; }
		
		[Field ("NSSharingServiceNameComposeEmail")][Internal]
		NSString NSSharingServiceNameComposeEmail { get; }
		
		[Field ("NSSharingServiceNameComposeMessage")][Internal]
		NSString NSSharingServiceNameComposeMessage { get; }
		
		[Field ("NSSharingServiceNameSendViaAirDrop")][Internal]
		NSString NSSharingServiceNameSendViaAirDrop { get; }
		
		[Field ("NSSharingServiceNameAddToSafariReadingList")][Internal]
		NSString NSSharingServiceNameAddToSafariReadingList { get; }
		
		[Field ("NSSharingServiceNameAddToIPhoto")][Internal]
		NSString NSSharingServiceNameAddToIPhoto { get; }
		
		[Field ("NSSharingServiceNameAddToAperture")][Internal]
		NSString NSSharingServiceNameAddToAperture { get; }
		
		[Field ("NSSharingServiceNameUseAsTwitterProfileImage")][Internal]
		NSString NSSharingServiceNameUseAsTwitterProfileImage { get; }
		
		[Field ("NSSharingServiceNameUseAsDesktopPicture")][Internal]
		NSString NSSharingServiceNameUseAsDesktopPicture { get; }
		
		[Field ("NSSharingServiceNamePostImageOnFlickr")][Internal]
		NSString NSSharingServiceNamePostImageOnFlickr { get; }
		
		[Field ("NSSharingServiceNamePostVideoOnVimeo")][Internal]
		NSString NSSharingServiceNamePostVideoOnVimeo { get; }
		
		[Field ("NSSharingServiceNamePostVideoOnYouku")][Internal]
		NSString NSSharingServiceNamePostVideoOnYouku { get; }
		
		[Field ("NSSharingServiceNamePostVideoOnTudou")][Internal]
		NSString NSSharingServiceNamePostVideoOnTudou { get; }
	}

	//64 bit reviewed
	[MountainLion]
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSSharingServiceDelegate 
	{
		[Export ("sharingService:willShareItems:"), EventArgs ("NSSharingServiceItems")]
		void WillShareItems (NSSharingService sharingService, NSObject [] items);
		
		[Export ("sharingService:didFailToShareItems:error:"), EventArgs ("NSSharingServiceDidFailToShareItems")]
		void DidFailToShareItems (NSSharingService sharingService, NSObject [] items, NSError error);
		
		[Export ("sharingService:didShareItems:"), EventArgs ("NSSharingServiceItems")]
		void DidShareItems (NSSharingService sharingService, NSObject [] items);
		
		[Export ("sharingService:sourceFrameOnScreenForShareItem:"), DelegateName ("NSSharingServiceSourceFrameOnScreenForShareItem"), DefaultValue (null)]
		CGRect SourceFrameOnScreenForShareItem (NSSharingService sharingService, NSPasteboardWriting item);

		[Export ("sharingService:transitionImageForShareItem:contentRect:"), DelegateName ("NSSharingServiceTransitionImageForShareItem"), DefaultValue (null)]
		NSImage TransitionImageForShareItem (NSSharingService sharingService, NSPasteboardWriting item, CGRect contentRect);

		[Export ("sharingService:sourceWindowForShareItems:sharingContentScope:"), DelegateName ("NSSharingServiceSourceWindowForShareItems"), DefaultValue (null)]
		NSWindow SourceWindowForShareItems (NSSharingService sharingService, NSObject [] items, NSSharingContentScope sharingContentScope);
	}
	
	[MountainLion]
	[BaseType (typeof (NSObject),
	           Delegates=new string [] {"WeakDelegate"},
	Events=new Type [] { typeof (NSSharingServicePickerDelegate) })]
	public interface NSSharingServicePicker 
	{
		[Export ("delegate")][NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Wrap ("WeakDelegate")][NullAllowed]
		NSSharingServicePickerDelegate Delegate { get; set; }
		
		[Export ("initWithItems:")]
		IntPtr Constructor (NSObject [] items);
		
		[Export ("showRelativeToRect:ofView:preferredEdge:")]
		void ShowRelativeToRect (CGRect rect, NSView view, NSRectEdge preferredEdge);
	}

	// 64bit reviewed
	[MountainLion]
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSSharingServicePickerDelegate 
	{
		[Export ("sharingServicePicker:sharingServicesForItems:proposedSharingServices:"), DelegateName ("NSSharingServicePickerSharingServicesForItems"), DefaultValueFromArgument ("proposedServices")]
		NSSharingService [] SharingServicesForItems (NSSharingServicePicker sharingServicePicker, NSObject [] items, NSSharingService [] proposedServices);
		
		[Export ("sharingServicePicker:delegateForSharingService:"), DelegateName ("NSSharingServicePickerDelegateForSharingService"), DefaultValue (null)]
		NSSharingServiceDelegate DelegateForSharingService (NSSharingServicePicker sharingServicePicker, NSSharingService sharingService);
		
		[Export ("sharingServicePicker:didChooseSharingService:"), EventArgs ("NSSharingServicePickerDidChooseSharingService")]
		void DidChooseSharingService (NSSharingServicePicker sharingServicePicker, NSSharingService service);
	}
	
	[Mavericks]
	[BaseType (typeof (NSObject))]
	public interface NSAppearance
	{
		[Static]
		[Export ("currentAppearance")]
		NSAppearance CurrentAppearance { get; [Bind ("setCurrentAppearance:")] set; }

		[Static]
		[Export ("appearanceNamed:")]
		NSAppearance GetAppearance(NSString name);
		
		[Field ("NSAppearanceNameAqua")]
		NSString NameAqua { get; }

		[Field ("NSAppearanceNameLightContent")]
		NSString NameLightContent { get; }
		
		[Field ("NSAppearanceNameVibrantDark")]
		NSString NameVibrantDark { get; }
		
		[Field ("NSAppearanceNameVibrantLight")]
		NSString NameVibrantLight { get; }
		
		[Export ("initWithAppearanceNamed:bundle:")]
		IntPtr Constructor (NSString name, NSBundle bundle);
		
		[Export ("name")]
		NSString Name { get; }
	
		[Export ("allowsVibrancy")]
		bool AllowsVibrancy { get; }
		
		[Mac (10, 14)]
		[Field ("NSAppearanceNameDarkAqua")]
		NSString NameDarkAqua { get; }
		
		[Mac (10, 14)]
		[Field ("NSAppearanceNameAccessibilityHighContrastAqua")]
		NSString NameAccessibilityHighContrastAqua { get; }

		[Mac (10, 14)]
		[Field ("NSAppearanceNameAccessibilityHighContrastDarkAqua")]
		NSString NameAccessibilityHighContrastDarkAqua { get; }

		[Mac (10, 14)]
		[Field ("NSAppearanceNameAccessibilityHighContrastVibrantLight")]
		NSString NameAccessibilityHighContrastVibrantLight { get; }

		[Mac (10, 14)]
		[Field ("NSAppearanceNameAccessibilityHighContrastVibrantDark")]
		NSString NameAccessibilityHighContrastVibrantDark { get; }

		[Mac (10,14)]
		[Export ("bestMatchFromAppearancesWithNames:")]
		[return: NullAllowed]
		string FindBestMatch (string[] appearances);
		
		[Mac (11, 0)]
		[Static]
		[Export ("currentDrawingAppearance", ArgumentSemantic.Strong)]
		NSAppearance CurrentDrawingAppearance { get; }

		[Mac (11,0)]
		[Export ("performAsCurrentDrawingAppearance:")]
		void PerformAsCurrentDrawingAppearance (Action receiver);
	}

	[Protocol]
	[Mac (10,11)]
	interface NSUserInterfaceValidations
	{
		[Abstract]
		[Export ("validateUserInterfaceItem:")]
		bool ValidateUserInterfaceItem (NSObject item);
	}

	partial interface NSResponder {
		[Export ("wantsScrollEventsForSwipeTrackingOnAxis:")]
		bool WantsScrollEventsForSwipeTrackingOnAxis (NSEventGestureAxis axis);

		[Export ("supplementalTargetForAction:sender:")]
		NSObject SupplementalTargetForAction (Selector action, [NullAllowed] NSObject sender);

		[Export ("smartMagnifyWithEvent:")]
		void SmartMagnify (NSEvent withEvent);

		[Export ("quickLookWithEvent:")]
		void QuickLook (NSEvent withEvent);

		// From  NSControlEditingSupport category. Needs to be here to make the API easier to be used. issue 4837
		[Export ("validateProposedFirstResponder:forEvent:")]
		bool ValidateProposedFirstResponder (NSResponder responder, [NullAllowed] NSEvent forEvent);

		[Mac (10,15)]
		[Export ("changeModeWithEvent:")]
		void ChangeMode (NSEvent withEvent);
	}

	[Mac (10,15)]
	[BaseType (typeof (NSToolbarItem))]
	interface NSMenuToolbarItem
	{
		[DesignatedInitializer]
		[Export ("initWithItemIdentifier:")]
		IntPtr Constructor (string itemIdentifier);

		[Export ("menu", ArgumentSemantic.Strong)]
		NSMenu Menu { get; set; }

		[Export ("showsIndicator")]
		bool ShowsIndicator { get; set; }
	}

}
