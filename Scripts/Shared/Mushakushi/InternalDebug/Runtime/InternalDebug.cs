// modified from: https://github.com/JoebRogers/UnityDebug/blob/master/src/InternalDebug_NoNamespace.cs
using System.Diagnostics;
using UnityEngine;

namespace Mushakushi.InternalDebug.Runtime
{
	/// <summary>
	/// Wrapper class of <see cref="UnityEngine.Debug"/> that strips calls any-non development build. 
	/// </summary>
	public static class InternalDebug
	{
		/// <summary>
		/// Assert a condition and logs an error message to the Unity console on failure.
		/// </summary>
		/// <param name="condition">Condition you expect to be true.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"), Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition)
		{
			UnityEngine.Debug.Assert(condition);
		}

		/// <summary>
		/// Assert a condition and logs an error message to the Unity console on failure.
		/// </summary>
		/// <param name="condition">Condition you expect to be true.</param>
		/// <param name="context">Object to which the message applies.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"), Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, Object context)
		{
			UnityEngine.Debug.Assert(condition, context);
		}

		/// <summary>
		/// Assert a condition and logs an error message to the Unity console on failure.
		/// </summary>
		/// <param name="condition">Condition you expect to be true.</param>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"), Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, object message)
		{
			UnityEngine.Debug.Assert(condition, message);
		}

		/// <summary>
		/// Assert a condition and logs an error message to the Unity console on failure.
		/// </summary>
		/// <param name="condition">Condition you expect to be true.</param>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"), Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, object message, Object context)
		{
			UnityEngine.Debug.Assert(condition, message, context);
		}

		/// <summary>
		/// Assert a condition and logs a formatted error message to the Unity console on failure.
		/// </summary>
		/// <param name="condition">Condition you expect to be true.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"), Conditional("UNITY_ASSERTIONS")]
		public static void AssertFormat(bool condition, string format, params object[] args)
		{
			UnityEngine.Debug.AssertFormat(condition, format, args);
		}

		/// <summary>
		/// Assert a condition and logs a formatted error message to the Unity console on failure.
		/// </summary>
		/// <param name="condition">Condition you expect to be true.</param>
		/// <param name="context">Object to which the message applies.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"), Conditional("UNITY_ASSERTIONS")]
		public static void AssertFormat(bool condition, Object context, string format, params object[] args)
		{
			UnityEngine.Debug.AssertFormat(condition, context, format, args);
		}

		/// <summary>
		/// Pauses the editor.
		/// </summary>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void Break()
		{
			UnityEngine.Debug.Break();
		}

		/// <summary>
		/// Clears errors from the developer console.
		/// </summary>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void ClearDeveloperConsole()
		{
			UnityEngine.Debug.ClearDeveloperConsole();
		}

		/// <summary>
		/// Draws a line between specified start and end points.
		/// </summary>
		/// <param name="start">Point in world space where the line should start.</param>
		/// <param name="end">Point in world space where the line should end.</param>
		/// <param name="color">Color of the line.</param>
		/// <param name="duration">How long the line should be visible for.</param>
		/// <param name="depthTest">Should the line be obscured by objects closer to the camera?</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
		{
			UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
		}

		/// <summary>
		/// Point in world space where the ray should start.
		/// </summary>
		/// <param name="start">Point in world space where the ray should start.</param>
		/// <param name="dir">Direction and length of the ray.</param>
		/// <param name="color">Color of the drawn line.</param>
		/// <param name="duration">How long the line will be visible for (in seconds).</param>
		/// <param name="depthTest">Should the line be obscured by other objects closer to the camera?</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
		{
			UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
		}

		/// <summary>
		/// Logs a message to the Unity Console.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void Log(object message)
		{
			UnityEngine.Debug.Log(message);
		}

		/// <summary>
		/// Logs a message to the Unity Console.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context"></param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void Log(object message, Object context)
		{
			UnityEngine.Debug.Log(message, context);
		}

		/// <summary>
		/// A variant of InternalDebug.Log that logs an assertion message to the console.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"), Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertion(object message)
		{
			UnityEngine.Debug.LogAssertion(message);
		}

		/// <summary>
		/// A variant of InternalDebug.Log that logs an assertion message to the console.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"), Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertion(object message, Object context)
		{ 
			UnityEngine.Debug.LogAssertion(message, context);
		}

		/// <summary>
		/// Logs a formatted assertion message to the Unity console.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"), Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertionFormat(string format, params object[] args)
		{
			UnityEngine.Debug.LogAssertionFormat(format, args);
		}

		/// <summary>
		/// Logs a formatted assertion message to the Unity console. 
		/// </summary>
		/// <param name="context">Object to which the message applies.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"), Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertionFormat(Object context, string format, params object[] args)
		{
			UnityEngine.Debug.LogAssertionFormat(context, format, args);
		}

		/// <summary>
		/// A variant of InternalDebug.Log that logs an error message to the console.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void LogError(object message)
		{
			UnityEngine.Debug.LogError(message);
		}

		/// <summary>
		/// A variant of InternalDebug.Log that logs an error message to the console.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void LogError(object message, Object context)
		{
			UnityEngine.Debug.LogError(message, context);
		}

		/// <summary>
		/// Logs a formatted error message to the Unity console.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void LogErrorFormat(string format, params object[] args)
		{
			UnityEngine.Debug.LogErrorFormat(format, args);
		}

		/// <summary>
		/// Logs a formatted error message to the Unity console.
		/// </summary>
		/// <param name="context">Object to which the message applies.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void LogErrorFormat(Object context, string format, params object[] args)
		{
			UnityEngine.Debug.LogErrorFormat(context, format, args);
		}

		/// <summary>
		/// A variant of InternalDebug.Log that logs an error message to the console. 
		/// </summary>
		/// <param name="exception">Runtime Exception.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void LogException(System.Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}

		/// <summary>
		/// A variant of InternalDebug.Log that logs an error message to the console.
		/// </summary>
		/// <param name="exception">Runtime Exception.</param>
		/// <param name="context">Object to which the message applies.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void LogException(System.Exception exception, Object context)
		{
			UnityEngine.Debug.LogException(exception, context);
		}

		/// <summary>
		/// Logs a formatted message to the Unity Console.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void LogFormat(string format, params object[] args)
		{
			UnityEngine.Debug.LogFormat(format, args);
		}

		/// <summary>
		/// Logs a formatted message to the Unity Console.
		/// </summary>
		/// <param name="context">Object to which the message applies.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void LogFormat(Object context, string format, params object[] args)
		{
			UnityEngine.Debug.LogFormat(context, format, args);
		}

		/// <summary>
		/// A variant of InternalDebug.Log that logs a warning message to the console.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void LogWarning(object message)
		{
			UnityEngine.Debug.LogWarning(message);
		}

		/// <summary>
		/// A variant of InternalDebug.Log that logs a warning message to the console.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void LogWarning(object message, Object context)
		{
			UnityEngine.Debug.LogWarning(message, context);
		}

		/// <summary>
		/// Logs a formatted warning message to the Unity Console.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void LogWarningFormat(string format, params object[] args)
		{
			UnityEngine.Debug.LogWarningFormat(format, args);
		}

		/// <summary>
		/// Logs a formatted warning message to the Unity Console.
		/// </summary>
		/// <param name="context">Object to which the message applies.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
		public static void LogWarningFormat(Object context, string format, params object[] args)
		{
			UnityEngine.Debug.LogWarningFormat(context, format, args);
		}
	}
}