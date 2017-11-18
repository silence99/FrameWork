#region License

/*
 * Copyright 2002-2010 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

#region Imports

using System;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;

#endregion

namespace Spring.Web.UI.Controls
{
#if !NET_2_0
	/// <summary>
	/// Represents a collection container that enables a <see cref="MultiView"/> control to maintain a list of its child controls.
	/// </summary>
	/// <author>Erich Eichinger</author>
	[
		AspNetHostingPermission(SecurityAction.LinkDemand, Level=AspNetHostingPermissionLevel.Minimal)
			, AspNetHostingPermission(SecurityAction.InheritanceDemand, Level=AspNetHostingPermissionLevel.Minimal)]
	public class ViewCollection : ControlCollection
	{
		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="owner"></param>
		public ViewCollection(Control owner) : base(owner)
		{
		}

		/// <summary>
		/// Add the specified <see cref="View"/> control to the collection.
		/// </summary>
		public override void Add(Control v)
		{
			if (!(v is View))
			{
				throw new ArgumentException("ViewCollection_must_contain_view");
			}
			base.Add(v);
		}

		/// <summary>
		/// Add the specified <see cref="View"/> control to the collection.
		/// </summary>
		public override void AddAt(int index, Control v)
		{
			if (!(v is View))
			{
				throw new ArgumentException("ViewCollection_must_contain_view");
			}
			base.AddAt(index, v);
		}

		/// <summary>
		/// Obtain the specified <see cref="View"/> control from the collection.
		/// </summary>
		public new View this[int i]
		{
			get { return (View) base[i]; }
		}
	}
#endif // !NET_2_0
}