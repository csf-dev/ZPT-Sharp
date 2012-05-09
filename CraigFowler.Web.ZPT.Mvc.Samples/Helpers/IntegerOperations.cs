//  
//  IntegerOperations.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2012 Craig Fowler
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using CraigFowler.Web.ZPT.Tales;

namespace CraigFowler.Web.ZPT.Mvc.Samples.Helpers
{
  public class IntegerOperations : ITalesNamespaceOperationModule
  {
    public string DefaultAlias {
      get {
        return "integer";
      }
    }
    
    public object FormatInteger(object input)
    {
      int typedInput = (int) input;
      return String.Format("over {0}", typedInput - 1);
    }
  }
}

