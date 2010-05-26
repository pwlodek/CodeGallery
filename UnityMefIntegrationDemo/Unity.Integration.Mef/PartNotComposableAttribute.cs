/// --------------------------------------------------------------------------------------
/// <copyright file="MefUnityContainerExtension.cs">
///     Copyright (C) 2008-2009 Piotr W³odek.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com, http://pwlodek.blogspot.com
/// </authors>
/// <summary>
///     Contains public PartNotComposableAttribute class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;

namespace Unity.Integration.Mef
{
    /// <summary>
    /// Suppresses MEF composition for a class created by the Unity container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PartNotComposableAttribute : Attribute
    {
        
    }
}