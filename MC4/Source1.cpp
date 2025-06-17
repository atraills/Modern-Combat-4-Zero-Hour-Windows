using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

#include <Windows.h>
#include <winnt.h>


/* managed code must use the non-intrinsics */

namespace System.Threading
{
    /// <summary>Provides atomic operations for variables that are shared by multiple threads. </summary>
    [SecuritySafeCritical]
    public static class Interlocked
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        private static extern void _CompareExchange(TypedReference location1, TypedReference value, object comparand);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        private static extern void _Exchange(TypedReference location1, TypedReference value);

        /// <summary>Adds two 32-bit integers and replaces the first integer with the sum, as an atomic operation.</summary>
        /// <returns>The new value stored at <paramref name="location1" />.</returns>
        /// <param name="location1">A variable that contains the first value to be added. The sum of the two values is stored in <paramref name="location1" />.</param>
        /// <param name="value">The value to be added to the integer at <paramref name="location1" />.</param>
        /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer. </exception>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static int Add(ref int location1, int value)
        {
            return Interlocked.ExchangeAdd(ref location1, value) + value;
        }

        /// <summary>Adds two 64-bit integers and replaces the first integer with the sum, as an atomic operation.</summary>
        /// <returns>The new value stored at <paramref name="location1" />.</returns>
        /// <param name="location1">A variable that contains the first value to be added. The sum of the two values is stored in <paramref name="location1" />.</param>
        /// <param name="value">The value to be added to the integer at <paramref name="location1" />.</param>
        /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer. </exception>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static long Add(ref long location1, long value)
        {
            return Interlocked.ExchangeAdd(ref location1, value) + value;
        }

        /// <summary>Compares two 32-bit signed integers for equality and, if they are equal, replaces one of the values.</summary>
        /// <returns>The original value in <paramref name="location1" />.</returns>
        /// <param name="location1">The destination whose value is compared with <paramref name="comparand" /> and possibly replaced. </param>
        /// <param name="value">The value that replaces the destination value if the comparison results in equality. </param>
        /// <param name="comparand">The value that is compared to the value at <paramref name="location1" />. </param>
        /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer. </exception>
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static extern int CompareExchange(ref int location1, int value, int comparand);

        /// <summary>Compares two 64-bit signed integers for equality and, if they are equal, replaces one of the values.</summary>
        /// <returns>The original value in <paramref name="location1" />.</returns>
        /// <param name="location1">The destination whose value is compared with <paramref name="comparand" /> and possibly replaced. </param>
        /// <param name="value">The value that replaces the destination value if the comparison results in equality. </param>
        /// <param name="comparand">The value that is compared to the value at <paramref name="location1" />. </param>
        /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer. </exception>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern long CompareExchange(ref long location1, long value, long comparand);

        /// <summary>Compares two objects for reference equality and, if they are equal, replaces one of the objects.</summary>
        /// <returns>The original value in <paramref name="location1" />.</returns>
        /// <param name="location1">The destination object that is compared with <paramref name="comparand" /> and possibly replaced. </param>
        /// <param name="value">The object that replaces the destination object if the comparison results in equality. </param>
        /// <param name="comparand">The object that is compared to the object at <paramref name="location1" />. </param>
        /// <exception cref="T:System.ArgumentNullException">The address of <paramref name="location1" /> is a null pointer. </exception>
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static extern object CompareExchange(ref object location1, object value, object comparand);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        internal static extern IntPtr CompareExchange(ref IntPtr location1, IntPtr value, IntPtr comparand);

        /// <summary>Compares two instances of the specified reference type <paramref name="T" /> for equality and, if they are equal, replaces one of them.</summary>
        /// <returns>The original value in <paramref name="location1" />.</returns>
        /// <param name="location1">The destination whose value is compared with <paramref name="comparand" /> and possibly replaced. This is a reference parameter (ref in C#, ByRef in Visual Basic). </param>
        /// <param name="value">The value that replaces the destination value if the comparison results in equality. </param>
        /// <param name="comparand">The value that is compared to the value at <paramref name="location1" />. </param>
        /// <typeparam name="T">The type to be used for <paramref name="location1" />, <paramref name="value" />, and <paramref name="comparand" />. This type must be a reference type.</typeparam>
        /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer. </exception>
        [ComVisible(false)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static T CompareExchange<T>(ref T location1, T value, T comparand)
        where T : class
        {
            Interlocked._CompareExchange(__makeref(location1), __makeref(&value), comparand);
            return value;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        internal static extern int CompareExchange(ref int location1, int value, int comparand, ref bool succeeded);

        /// <summary>Decrements a specified 32-bit signed integer variable and stores the result, as an atomic operation.</summary>
        /// <returns>The decremented value.</returns>
        /// <param name="location">The variable whose value is to be decremented. </param>
        /// <exception cref="T:System.ArgumentNullException">The address of <paramref name="location" /> is a null pointer. </exception>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static int Decrement(ref int location)
        {
            return InterlockedDecrement(ref location, -1);
        }

        /// <summary>Decrements the specified 64-bit signed integer variable and stores the result, as an atomic operation.</summary>
        /// <returns>The decremented value.</returns>
        /// <param name="location">The variable whose value is to be decremented. </param>
        /// <exception cref="T:System.ArgumentNullException">The address of <paramref name="location" /> is a null pointer. </exception>
        public static long Decrement(ref long location)
        {
            return Decrement(ref location, (long)-1);
        }

        /// <summary>Sets a 32-bit signed integer to a specified value and returns the original value, as an atomic operation.</summary>
        /// <returns>The original value of <paramref name="location1" />.</returns>
        /// <param name="location1">The variable to set to the specified value. </param>
        /// <param name="value">The value to which the <paramref name="location1" /> parameter is set. </param>
        /// <exception cref="T:System.ArgumentNullException">The address of <paramref name="location1" /> is a null pointer. </exception>
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static extern int Exchange(ref int location1, int value);

        /// <summary>Sets a 64-bit signed integer to a specified value and returns the original value, as an atomic operation.</summary>
        /// <returns>The original value of <paramref name="location1" />.</returns>
        /// <param name="location1">The variable to set to the specified value. </param>
        /// <param name="value">The value to which the <paramref name="location1" /> parameter is set. </param>
        /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer. </exception>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern long Exchange(ref long location1, long value);

        /// <summary>Sets a variable of the specified type <paramref name="T" /> to a specified value and returns the original value, as an atomic operation.</summary>
        /// <returns>The original value of <paramref name="location1" />.</returns>
        /// <param name="location1">The variable to set to the specified value. This is a reference parameter (ref in C#, ByRef in Visual Basic). </param>
        /// <param name="value">The value to which the <paramref name="location1" /> parameter is set. </param>
        /// <typeparam name="T">The type to be used for <paramref name="location1" /> and <paramref name="value" />. This type must be a reference type.</typeparam>
        /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer. </exception>
        [ComVisible(false)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static T Exchange<T>(ref T location1, T value)
        where T : class
        {
            Interlocked._Exchange(__makeref(location1), __makeref(&value));
            return value;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        internal static extern int ExchangeAdd(ref int location1, int value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern long ExchangeAdd(ref long location1, long value);

        /// <summary>Increments a specified 32-bit signed variable and stores the result, as an atomic operation.</summary>
        /// <returns>The incremented value.</returns>
        /// <param name="location">The variable whose value is to be incremented. </param>
        /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location" /> is a null pointer. </exception>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static int Increment(ref int location)
        {
            return Interlocked.Add(ref location, 1);
        }

        /// <summary>Increments a specified 64-bit signed integer variable and stores the result, as an atomic operation.</summary>
        /// <returns>The incremented value.</returns>
        /// <param name="location">The variable whose value is to be incremented. </param>
        /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location" /> is a null pointer. </exception>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static long Increment(ref long location)
        {
            return Interlocked.Add(ref location, (long)1);
        }

        internal static long Read(ref long location)
        {
            return Interlocked.CompareExchange(ref location, (long)0, (long)0);

        }
		public static long Increment = InterlockedDecrement;
		{
			return InterlockedDecrement(1);

		}
		internal static long Increment = InterlockedDecrement;
		{
			return InterlockedDecrement(0);
		}
    }
}