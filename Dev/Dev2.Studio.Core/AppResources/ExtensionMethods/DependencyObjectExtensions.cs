/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;


namespace Dev2.Studio.Core.AppResources.ExtensionMethods
{
    public static class DependencyObjectExtensions
    {
        public static IEnumerable<DependencyObject> GetDescendents(this DependencyObject dependencyObject)
        {
            var descendents = new List<DependencyObject>();

            if (dependencyObject == null)
            {
                return descendents;
            }
            for(int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                descendents.Add(VisualTreeHelper.GetChild(dependencyObject, i));
            }

            foreach(DependencyObject descendent in descendents.ToList())
            {
                descendents.AddRange(GetDescendents(descendent));
            }

            return descendents;
        }

        public static DependencyObject GetChildByType(DependencyObject source, Type type)
        {
            for(int i = 0; i < VisualTreeHelper.GetChildrenCount(source); i++)
            {
                var child = VisualTreeHelper.GetChild(source, i);

                if (child.GetType() == type)
                {
                    return child;
                }
            }

            for(int i = 0; i < VisualTreeHelper.GetChildrenCount(source); i++)
            {
                var child = VisualTreeHelper.GetChild(source, i);
                var nestedchild = GetChildByType(child, type);
                if (nestedchild != null)
                {
                    return nestedchild;
                }
            }

            return null;
        }

        public static DependencyObject GetParentByType(this DependencyObject source, Type type)
        {
            var parent = VisualTreeHelper.GetParent(source);

            if (parent == null)
            {
                return null;
            }

            return parent.GetType() == type ? parent : GetParentByType(parent, type);
        }

        /// <summary>
        /// Find the first child of the specified type (the child must exist)
        /// by walking down the logical/visual trees
        /// Will throw an exception if a matching child does not exist. If you're not sure, use the TryFindChild method instead.
        /// </summary>
        /// <typeparam name="T">The type of child you want to find</typeparam>
        /// <param name="parent">The dependency object whose children you wish to scan</param>
        /// <returns>The first descendant of the specified type</returns>
        /// <remarks> usage: myWindow.FindChild<StackPanel>() </StackPanel></remarks>
        public static T FindChild<T>(this DependencyObject parent)
            where T : DependencyObject => parent.FindChild<T>(child => true);

        /// <summary>
        /// Find the first child of the specified type (the child must exist)
        /// by walking down the logical/visual trees, which meets the specified criteria
        /// Will throw an exception if a matching child does not exist. If you're not sure, use the TryFindChild method instead.
        /// </summary>
        /// <typeparam name="T">The type of child you want to find</typeparam>
        /// <param name="parent">The dependency object whose children you wish to scan</param>
        /// <param name="predicate">The child object is selected if the predicate evaluates to true</param>
        /// <returns>The first matching descendant of the specified type</returns>
        /// <remarks> usage: myWindow.FindChild<StackPanel>( child => child.Name == "myPanel" ) </StackPanel></remarks>
        public static T FindChild<T>(this DependencyObject parent, Func<T, bool> predicate)
            where T : DependencyObject
        {
            var findChildren = parent.FindChildren(predicate);
            return findChildren.FirstOrDefault();
        }

        /// <summary>
        /// Get a list of descendant dependencyobjects of the specified type and which meet the criteria
        /// as specified by the predicate
        /// </summary>
        /// <typeparam name="T">The type of child you want to find</typeparam>
        /// <param name="parent">The dependency object whose children you wish to scan</param>
        /// <param name="predicate">The child object is selected if the predicate evaluates to true</param>
        /// <returns>The first matching descendant of the specified type</returns>
        /// <remarks> usage: myWindow.FindChildren<StackPanel>( child => child.Name == "myPanel" ) </StackPanel></remarks>
        public static IEnumerable<T> FindChildren<T>(this DependencyObject parent, Func<T, bool> predicate)
            where T : DependencyObject
        {
            var children = new List<DependencyObject>();

            if(parent is Visual || parent is Visual3D)
            {
                var visualChildrenCount = VisualTreeHelper.GetChildrenCount(parent);
                for(int childIndex = 0; childIndex < visualChildrenCount; childIndex++)
                {
                    children.Add(VisualTreeHelper.GetChild(parent, childIndex));
                }
            }
            foreach(var logicalChild in LogicalTreeHelper.GetChildren(parent).OfType<DependencyObject>())
            {
                if (!children.Contains(logicalChild))
                {
                    children.Add(logicalChild);
                }
            }

            foreach (var child in children)
            {
                if ((child is T typedChild) && predicate.Invoke(typedChild))
                {
                    yield return typedChild;
                }

                foreach (var foundDescendant in FindChildren(child, predicate))
                {
                    yield return foundDescendant;
                }
            }
        }
    }
}
