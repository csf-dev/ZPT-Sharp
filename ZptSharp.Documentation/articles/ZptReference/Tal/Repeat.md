# The `tal:repeat` attribute

A `tal:repeat` attribute is used to repeat a subtree of the markup (an element and all of its content & descendents) for every item within an `IEnumerable` collection.
Developers may think of this as being similar to a `foreach` loop.

The syntax of a repeat attribute is comprised of:

1. A variable name
2. The TALES expression which is an `IEnumerable` collection

The variable name must follow [the same rules as a `tal:define` variable name], notably it must be a valid C# variable name.
Additionally, if the TALES expression does not evaluate to one of null, or an object which implements `IEnumerable`, or an instance of 

[the same rules as a `tal:define` variable name]: Define.md#variable-names

## The element is duplicated for each item in the collection

The functionality of the `tal:repeat` attribute will duplicate the entire subtree (starting with the element upon which the `tal:repeat` attribute is present) once for every item in the collection.
All TAL attributes in that subtree are also duplicated and may thus be evaluated many times.

In each iteration, [a local variable] is created using the variable name specified in the `tal:repeat` attribute.
That variable's value is set to the item from the current iteration of the collection.
The iterations and the variable values use the exact same order as which the `IEnumerable` returns them.
Any inherently-ordered collections such as `IList<T>` will have their order respected by repeat attribute.

All other TAL attributes on the element or its descendents (including those which have been duplicated) are handled as-normal as if the template source had originally been written with those repetitions hard-coded.

[a local variable]: Define.md#local-variables

## Beware of order-of-operations

[As noted in the table listing all TAL attributes], the `tal:repeat` attribute is processed _after_ [`tal:define` attributes] & [`tal:condition` attributes].
This creates _a very easy-to-make mistake_ as shown in the following code snippet:

```html
<ul tal:repeat="item here/someItems"
    tal:define="itemName item/name">
    <li tal:content="itemName">Item name</li>
</ul>
```

Can you see the mistake?
Even though the `tal:repeat` attribute is written before the `tal:define` attribute, _it will be processed afterwards_.
This means that the `tal:define` attribute will attempt to evaluate the expression `item/name` before the `item` variable has been created, and will almost surely raise an error.

[As noted in the table listing all TAL attributes]: ../Index.md#tal-binds-data-to-the-template
[`tal:define` attributes]: Define.md
[`tal:condition` attributes]: Condition.md

## If the collection is empty

If the expression evaluates to an `IEnumerable` which is empty (has no iterations) then the subtree beginning with the element which has the `tal:repeat` attribute _is removed from the rendered output entirely_ and is not processed.
In other words, no items means no repetitions of the markup.

## If the collection is null

If the expression evaluates to a null reference, then the behaviour is exactly the same as if the action were aborted, as described below.

## Aborting a `tal:repeat` attribute

If the expression evaluates to [an instance of `AbortZptActionToken`], such as via [the root context `default`] then the subtree beginning with the element which has the `tal:repeat` attribute is left as-is.
Also, when a repeat attribute is aborted, no variable is defined for the current iteration.
Be aware that this could cause errors when ZPT processes TAL attributes upon descendent elements, if they attempt to make use of the 'current iteration' variable.

[an instance of `AbortZptActionToken`]: xref:ZptSharp.Expressions.AbortZptActionToken
[the root context `default`]: ../Tales/GlobalContexts.md#default

## If the expression result is not `IEnumerable`

If the expression result is not `IEnumerable` and is also neither null or [an instance of `AbortZptActionToken`] then ZptSharp will raise an error.

## Repeat variables

As well as a local variable, metadata about the current iteration is also accessible from [a special root context named `repeat`].
The contents of the `repeat` root context are a collection of named objects corresponding to all of the 'current iteration' variables which are currently in-scope.
Each is named using the variable-name given in its corresponding `tal:repeat` attribute.

Each of these 'repeat objects' provides the following properties:

| Property  | Meaning                                                                       |
| --------  | -------                                                                       |
| `index`   | The zero-based index of the current iteration                                 |
| `number`  | The one-based index of the current iteration (equivalent to `index + 1`)      |
| `even`    | `true` if the `index` is an even number (0, 2, 4 etc), `false` otherwise      |
| `odd`     | `true` if the `index` is an odd number (1, 3, 5 etc), `false` otherwise       |
| `start`   | `true` if the `index` is zero, `false` otherwise                              |
| `end`     | `true` if the current iteration is the last, `false` otherwise                |
| `length`  | The count of all items in the collection                                      |
| `letter`  | A string 'letter reference' for the current item: a, b, c, ... aa, ab, ac etc |
| `Letter`  | The uppercase equivalent of `letter`                                          |
| `roman`   | A lowercase roman-numeral representation of `number`: i, ii, iii, iv etc      |
| `Roman`   | The uppercase equivalent of `roman`                                           |

[a special root context named `repeat`]: ../Tales/GlobalContexts.md#repeat

## Examples

### A table showing the roman numerals for 1 to 10

Let's presume that the model expression `here/oneToTen` holds the result of `Enumerable.Range(1, 10)` (using `Enumerable` from the `System.Linq` namespace).
The following example would create a table showing the decimal number on the first column and its uppercase roman numeral equivalent on the second column.

```html
<table>
    <thead>
        <tr>
            <th>Decimal</th>
            <th>Roman</th>
        </tr>
    </thead>
    <tbody>
        <tr tal:repeat="number here/oneToTen">
            <td tal:content="number">Decimal number</td>
            <td tal:content="repeat/roman">Roman number</td>
        </tr>
    </tbody>
</table>
```

### A list of items in a shopping cart

Let's presume that the model expression `here/shoppingCartItems` holds a collection of objects which are instances of the following class.

```csharp
public class ShoppingCartItem
{
    public int Quantity { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
}
```

Here is some sample markup for a list of shopping cart items.

```html
<ul>
    <li tal:repeat="item here/shoppingCartItems">
        <img src="images/placeholder.png" tal:attributes="src item/ImageUrl">
        <span class="name" tal:content="item/Name">Item name</span>
        <label for="itemQuantity_0"
               tal:attributes="for string:itemQuantity_${repeat/item/number}">Quantity</label>
        <input class="quantity"
               id="itemQuantity_0"
               value="0"
               type="text"
               tal:attributes="value item/Quantity;
                               id string:itemQuantity_${repeat/item/number}">
    </li>
</ul>
```

This example brings together a number of techniques:

* The `src` attribute for the image is replaced using [a `tal:attributes` attribute]
* The item name is filled-in using [a `tal:content` attribute]
* The `<input>` element for the quantity is assigned a unique `id` attribute by using the current repetition-number
* The `<label>` element has its `for` attribute set to the same value as the `id` of the corresponding `<input>` element

[a `tal:attributes` attribute]: Attributes.md
[a `tal:content` attribute]: ContentAndReplace.md
