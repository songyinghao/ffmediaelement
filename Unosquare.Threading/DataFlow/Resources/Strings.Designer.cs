﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Unosquare.Threading.DataFlow.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Unosquare.Threading.DataFlow.Resources.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BoundedCapacity must be Unbounded or -1 for this dataflow block..
        /// </summary>
        internal static string Argument_BoundedCapacityNotSupported {
            get {
                return ResourceManager.GetString("Argument_BoundedCapacityNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The argument must be false if no source from which to consume is specified..
        /// </summary>
        internal static string Argument_CantConsumeFromANullSource {
            get {
                return ResourceManager.GetString("Argument_CantConsumeFromANullSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The DataflowMessageHeader instance does not represent a valid message header..
        /// </summary>
        internal static string Argument_InvalidMessageHeader {
            get {
                return ResourceManager.GetString("Argument_InvalidMessageHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to To construct a DataflowMessageHeader instance, either pass a non-zero value or use the parameterless constructor..
        /// </summary>
        internal static string Argument_InvalidMessageId {
            get {
                return ResourceManager.GetString("Argument_InvalidMessageId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This block must only be used with the source from which it was created..
        /// </summary>
        internal static string Argument_InvalidSourceForFilteredLink {
            get {
                return ResourceManager.GetString("Argument_InvalidSourceForFilteredLink", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Greedy must be true for this dataflow block..
        /// </summary>
        internal static string Argument_NonGreedyNotSupported {
            get {
                return ResourceManager.GetString("Argument_NonGreedyNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Number must be no greater than the value specified in BoundedCapacity..
        /// </summary>
        internal static string ArgumentOutOfRange_BatchSizeMustBeNoGreaterThanBoundedCapacity {
            get {
                return ResourceManager.GetString("ArgumentOutOfRange_BatchSizeMustBeNoGreaterThanBoundedCapacity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Number must be positive..
        /// </summary>
        internal static string ArgumentOutOfRange_GenericPositive {
            get {
                return ResourceManager.GetString("ArgumentOutOfRange_GenericPositive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Number must be either non-negative and less than or equal to Int32.MaxValue or -1.
        /// </summary>
        internal static string ArgumentOutOfRange_NeedNonNegOrNegative1 {
            get {
                return ResourceManager.GetString("ArgumentOutOfRange_NeedNonNegOrNegative1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Object is not a array with the same initialization state as the array to compare it to..
        /// </summary>
        internal static string ArrayInitializedStateNotEqual {
            get {
                return ResourceManager.GetString("ArrayInitializedStateNotEqual", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Object is not a array with the same number of elements as the array to compare it to..
        /// </summary>
        internal static string ArrayLengthsNotEqual {
            get {
                return ResourceManager.GetString("ArrayLengthsNotEqual", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find the old value.
        /// </summary>
        internal static string CannotFindOldValue {
            get {
                return ResourceManager.GetString("CannotFindOldValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Capacity was less than the current Count of elements..
        /// </summary>
        internal static string CapacityMustBeGreaterThanOrEqualToCount {
            get {
                return ResourceManager.GetString("CapacityMustBeGreaterThanOrEqualToCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MoveToImmutable can only be performed when Count equals Capacity..
        /// </summary>
        internal static string CapacityMustEqualCountOnMove {
            get {
                return ResourceManager.GetString("CapacityMustEqualCountOnMove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Collection was modified; enumeration operation may not execute..
        /// </summary>
        internal static string CollectionModifiedDuringEnumeration {
            get {
                return ResourceManager.GetString("CollectionModifiedDuringEnumeration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The SyncRoot property may not be used for the synchronization of concurrent collections..
        /// </summary>
        internal static string ConcurrentCollection_SyncRoot_NotSupported {
            get {
                return ResourceManager.GetString("ConcurrentCollection_SyncRoot_NotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An element with the same key but a different value already exists. Key: {0}.
        /// </summary>
        internal static string DuplicateKey {
            get {
                return ResourceManager.GetString("DuplicateKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Block {0} completed as {1}. {2}.
        /// </summary>
        internal static string event_DataflowBlockCompleted {
            get {
                return ResourceManager.GetString("event_DataflowBlockCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Block of type {0} instantiated with Id {1}..
        /// </summary>
        internal static string event_DataflowBlockCreated {
            get {
                return ResourceManager.GetString("event_DataflowBlockCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Source {0} linked to target {1}..
        /// </summary>
        internal static string event_DataflowBlockLinking {
            get {
                return ResourceManager.GetString("event_DataflowBlockLinking", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Source {0} unlinked from target {1}..
        /// </summary>
        internal static string event_DataflowBlockUnlinking {
            get {
                return ResourceManager.GetString("event_DataflowBlockUnlinking", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {1} task launched from block {0} with {2} message(s) pending..
        /// </summary>
        internal static string event_TaskLaunchedForMessageHandling {
            get {
                return ResourceManager.GetString("event_TaskLaunchedForMessageHandling", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This operation does not apply to an empty instance..
        /// </summary>
        internal static string InvalidEmptyOperation {
            get {
                return ResourceManager.GetString("InvalidEmptyOperation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The source completed without providing data to receive..
        /// </summary>
        internal static string InvalidOperation_DataNotAvailableForReceive {
            get {
                return ResourceManager.GetString("InvalidOperation_DataNotAvailableForReceive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The target block failed to consume a message it had successfully reserved..
        /// </summary>
        internal static string InvalidOperation_FailedToConsumeReservedMessage {
            get {
                return ResourceManager.GetString("InvalidOperation_FailedToConsumeReservedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The target does not have the message reserved..
        /// </summary>
        internal static string InvalidOperation_MessageNotReservedByTarget {
            get {
                return ResourceManager.GetString("InvalidOperation_MessageNotReservedByTarget", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This operation cannot be performed on a default instance of ImmutableArray&lt;T&gt;.  Consider initializing the array, or checking the ImmutableArray&lt;T&gt;.IsDefault property..
        /// </summary>
        internal static string InvalidOperationOnDefaultArray {
            get {
                return ResourceManager.GetString("InvalidOperationOnDefaultArray", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This member is not supported on this dataflow block. The block is intended for a specific purpose that does not utilize this member..
        /// </summary>
        internal static string NotSupported_MemberNotNeeded {
            get {
                return ResourceManager.GetString("NotSupported_MemberNotNeeded", resourceCulture);
            }
        }
    }
}
