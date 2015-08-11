// <copyright file="CodeAnnotations.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/02/06</date>
// <summary>
// 
// </summary>

using System;
using System.CodeDom.Compiler;
using System.Linq;



namespace TheQ.Utilities.CloudTools.Storage.Internal
{
#pragma warning disable 1591

	// ReSharper disable UnusedMember.Global
	// ReSharper disable UnusedParameter.Local
	// ReSharper disable MemberCanBePrivate.Global
	// ReSharper disable UnusedAutoPropertyAccessor.Global
	// ReSharper disable IntroduceOptionalParameters.Global
	// ReSharper disable MemberCanBeProtected.Global
	// ReSharper disable InconsistentNaming


	/// <summary>
	///     <para>Indicates that the value of the marked element could be <c>null</c></para>
	///     <para>sometimes, so the check for <c>null</c></para>
	///     <para>is necessary before its usage</para>
	/// </summary>
	/// <example>
	///     <code>
	/// [CanBeNull] internal object Test() { return null; }
	/// internal void UseTest() {
	///   var p = Test();
	///   var s = p.ToString(); // Warning: Possible 'System.NullReferenceException'
	/// }
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class CanBeNullAttribute : Attribute
	{
	}



	/// <summary>
	///     Indicates that the value of the marked element could never be <c>null</c>
	/// </summary>
	/// <example>
	///     <code>
	/// [NotNull] internal object Foo() {
	///   return null; // Warning: Possible 'null' assignment
	/// }
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class NotNullAttribute : Attribute
	{
	}



	/// <summary>
	///     <para>
	///         Indicates that the marked method builds string by format pattern and (optional) arguments. Parameter, which contains format string, should be given in constructor. The format string should be
	///         in
	///         <see cref="String.Format(System.IFormatProvider,System.String,System.Object[])" />
	///     </para>
	///     <para>-like form</para>
	/// </summary>
	/// <example>
	///     <code>
	/// [StringFormatMethod("message")]
	/// internal void ShowError(string message, params object[] args) { /* do something */ }
	/// internal void Foo() {
	///   ShowError("Failed: {0}"); // Warning: Non-existing argument in format string
	/// }
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class StringFormatMethodAttribute : Attribute
	{
		/// <param name="formatParameterName">Specifies which parameter of an annotated method should be treated as format-string</param>
		internal StringFormatMethodAttribute(string formatParameterName) { this.FormatParameterName = formatParameterName; }



		internal string FormatParameterName { get; private set; }
	}



	/// <summary>
	///     Indicates that the function argument should be string literal and match one of the parameters of the caller function. For example, ReSharper annotates the parameter of
	///     <see cref="ArgumentNullException" />
	/// </summary>
	/// <example>
	///     <code>
	/// internal void Foo(string param) {
	///   if (param == null)
	///     throw new ArgumentNullException("par"); // Warning: Cannot resolve symbol
	/// }
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.Parameter)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class InvokerParameterNameAttribute : Attribute
	{
	}



	/// <summary>
	///     <para>Indicates that the method is contained in a type that implements <see cref="System.ComponentModel.INotifyPropertyChanged" /></para>
	///     <para><see langword="interface" /> and this method is used to notify that some property value changed</para>
	/// </summary>
	/// <remarks>
	///     <para>The method should be non-static and conform to one of the supported signatures:</para>
	///     <list type="bullet">
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///     </list>
	/// </remarks>
	/// <example>
	///     <code>
	///  internal class Foo : INotifyPropertyChanged {
	///    internal event PropertyChangedEventHandler PropertyChanged;
	///    [NotifyPropertyChangedInvocator]
	///    protected virtual void NotifyChanged(string propertyName) { ... }
	/// 
	///    private string _name;
	///    internal string Name {
	///      get { return _name; }
	///      set { _name = value; NotifyChanged("LastName"); /* Warning */ }
	///    }
	///  }
	///  </code>
	///     <para>Examples of generated notifications:</para>
	///     <list type="bullet">
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///     </list>
	/// </example>
	[AttributeUsage(AttributeTargets.Method)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
	{
		internal NotifyPropertyChangedInvocatorAttribute() { }


		internal NotifyPropertyChangedInvocatorAttribute(string parameterName) { this.ParameterName = parameterName; }


		internal string ParameterName { get; private set; }
	}



	/// <summary>
	///     Describes dependency between method input and output
	/// </summary>
	/// <syntax>
	///     <para>
	///         Function Definition Table syntax:
	///     </para>
	///     <list type="bullet">
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///     </list>
	///     If method has single input parameter, it's name could be omitted.
	///     <br />
	///     Using
	///     <c>halt</c>
	///     (or
	///     <c>void</c>
	///     /
	///     <c>nothing</c>
	///     , which is the same) for method output means that the methos doesn't return normally.
	///     <br /> <c>canbenull</c>
	///     annotation is only applicable for output parameters.
	///     <br />
	///     You can use multiple
	///     <c>[ContractAnnotation]</c>
	///     for each FDT row, or use single attribute with rows separated by semicolon.
	///     <br />
	/// </syntax>
	/// <examples>
	///     <list type="bullet">
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item></item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///         <item>
	///             <description></description>
	///         </item>
	///     </list>
	/// </examples>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class ContractAnnotationAttribute : Attribute
	{
		internal ContractAnnotationAttribute([NotNull] string contract)
			: this(contract, false) { }



		internal ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
		{
			this.Contract = contract;
			this.ForceFullStates = forceFullStates;
		}



		internal string Contract { get; private set; }


		internal bool ForceFullStates { get; private set; }
	}



	/// <summary>
	///     Indicates that marked element should be localized or not
	/// </summary>
	/// <example>
	///     <code>
	/// [LocalizationRequiredAttribute(true)]
	/// internal class Foo {
	///   private string str = "my string"; // Warning: Localizable string
	/// }
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.All)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class LocalizationRequiredAttribute : Attribute
	{
		internal LocalizationRequiredAttribute()
			: this(true) { }



		internal LocalizationRequiredAttribute(bool required) { this.Required = required; }


		internal bool Required { get; private set; }
	}



	/// <summary>
	///     <para>Indicates that the value of the marked type (or its derivatives) cannot be compared using '==' or '!=' operators and <c>Equals()</c></para>
	///     <para>should be used instead. However, using '==' or '!=' for comparison with <c>null</c></para>
	///     <para>is always permitted.</para>
	/// </summary>
	/// <example>
	///     <code>
	/// [CannotApplyEqualityOperator]
	/// class NoEquality { }
	/// class UsesNoEquality {
	///   internal void Test() {
	///     var ca1 = new NoEquality();
	///     var ca2 = new NoEquality();
	///     if (ca1 != null) { // OK
	///       bool condition = ca1 == ca2; // Warning
	///     }
	///   }
	/// }
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class CannotApplyEqualityOperatorAttribute : Attribute
	{
	}



	/// <summary>
	///     When applied to a target attribute, specifies a requirement for any type marked with the target attribute to implement or inherit specific type or types.
	/// </summary>
	/// <example>
	///     <code>
	/// [BaseTypeRequired(typeof(IComponent)] // Specify requirement
	/// internal class ComponentAttribute : Attribute { }
	/// [Component] // ComponentAttribute requires implementing IComponent interface
	/// internal class MyComponent : IComponent { }
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	[BaseTypeRequired(typeof (Attribute))]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class BaseTypeRequiredAttribute : Attribute
	{
		internal BaseTypeRequiredAttribute([NotNull] Type baseType) { this.BaseType = baseType; }


		[NotNull]
		internal Type BaseType { get; private set; }
	}



	/// <summary>
	///     Indicates that the marked symbol is used implicitly (e.g. via reflection, in external library), so this symbol will not be marked as unused (as well as by other usage inspections)
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class UsedImplicitlyAttribute : Attribute
	{
		internal UsedImplicitlyAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default) { }



		internal UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default) { }



		internal UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags) { }



		internal UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			this.UseKindFlags = useKindFlags;
			this.TargetFlags = targetFlags;
		}



		internal ImplicitUseTargetFlags TargetFlags { get; private set; }


		internal ImplicitUseKindFlags UseKindFlags { get; private set; }
	}



	/// <summary>
	///     Should be used on attributes and causes ReSharper to not mark symbols marked with such attributes as unused (as well as by other usage inspections)
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class MeansImplicitUseAttribute : Attribute
	{
		internal MeansImplicitUseAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default) { }



		internal MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default) { }



		internal MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags) { }



		internal MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			this.UseKindFlags = useKindFlags;
			this.TargetFlags = targetFlags;
		}



		[UsedImplicitly]
		internal ImplicitUseTargetFlags TargetFlags { get; private set; }


		[UsedImplicitly]
		internal ImplicitUseKindFlags UseKindFlags { get; private set; }
	}



	[Flags]
	[GeneratedCode("Resharper", "8.1+")]
	internal enum ImplicitUseKindFlags
	{
		Default = Access | Assign | InstantiatedWithFixedConstructorSignature,


		/// <summary>
		///     Only entity marked with attribute considered used
		/// </summary>
		Access = 1,


		/// <summary>
		///     Indicates <see langword="implicit" /> assignment to a member
		/// </summary>
		Assign = 2,


		/// <summary>
		///     Indicates <see langword="implicit" /> instantiation of a type with fixed constructor signature. That means any unused constructor parameters won't be reported as such.
		/// </summary>
		InstantiatedWithFixedConstructorSignature = 4,


		/// <summary>
		///     Indicates <see langword="implicit" /> instantiation of a type
		/// </summary>
		InstantiatedNoFixedConstructorSignature = 8
	}



	/// <summary>
	///     <para>Specify what is considered used implicitly when marked with <see cref="MeansImplicitUseAttribute" /></para>
	///     <para>or <see cref="UsedImplicitlyAttribute" /></para>
	/// </summary>
	[Flags]
	[GeneratedCode("Resharper", "8.1+")]
	internal enum ImplicitUseTargetFlags
	{
		Default = Itself,


		Itself = 1,


		/// <summary>
		///     <see cref="TheQ.Utilities.CloudTools.Storage.Internal.ImplicitUseTargetFlags.Members" /> of entity marked with attribute are considered used
		/// </summary>
		Members = 2,


		/// <summary>
		///     Entity marked with attribute and all its members considered used
		/// </summary>
		WithMembers = Itself | Members
	}



	/// <summary>
	///     This attribute is intended to mark publicly available API which should not be removed and so is treated as used
	/// </summary>
	[MeansImplicitUse]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class PublicAPIAttribute : Attribute
	{
		internal PublicAPIAttribute() { }


		internal PublicAPIAttribute([NotNull] string comment) { this.Comment = comment; }


		[NotNull]
		internal string Comment { get; private set; }
	}



	/// <summary>
	///     Tells code analysis engine if the parameter is completely handled when the invoked method is on stack. If the parameter is a delegate, indicates that <see langword="delegate" /> is executed while
	///     the method is executed. If the parameter is an enumerable, indicates that it is enumerated while the method is executed
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class InstantHandleAttribute : Attribute
	{
	}



	/// <summary>
	///     Indicates that a method does not make any observable state changes. The same as <c>System.Diagnostics.Contracts.PureAttribute</c>
	/// </summary>
	/// <example>
	///     <code>
	/// [Pure] private int Multiply(int x, int y) { return x * y; }
	/// internal void Foo() {
	///   const int a = 2, b = 2;
	///   Multiply(a, b); // Waring: Return value of pure method is not used
	/// }
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.Method)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class PureAttribute : Attribute
	{
	}



	/// <summary>
	///     Indicates that a parameter is a path to a file or a folder within a web project. Path can be relative or absolute, starting from web root (~)
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	[GeneratedCode("Resharper", "8.1+")]
	public class PathReferenceAttribute : Attribute
	{
		internal PathReferenceAttribute() { }


		internal PathReferenceAttribute([PathReference] string basePath) { this.BasePath = basePath; }


		[NotNull]
		internal string BasePath { get; private set; }
	}



	// ASP.NET MVC attributes

	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcAreaMasterLocationFormatAttribute : Attribute
	{
		internal AspMvcAreaMasterLocationFormatAttribute(string format) { }
	}



	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcAreaPartialViewLocationFormatAttribute : Attribute
	{
		internal AspMvcAreaPartialViewLocationFormatAttribute(string format) { }
	}



	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcAreaViewLocationFormatAttribute : Attribute
	{
		internal AspMvcAreaViewLocationFormatAttribute(string format) { }
	}



	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcMasterLocationFormatAttribute : Attribute
	{
		internal AspMvcMasterLocationFormatAttribute(string format) { }
	}



	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcPartialViewLocationFormatAttribute : Attribute
	{
		internal AspMvcPartialViewLocationFormatAttribute(string format) { }
	}



	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcViewLocationFormatAttribute : Attribute
	{
		internal AspMvcViewLocationFormatAttribute(string format) { }
	}



	/// <summary>
	///     ASP.NET MVC attribute. If applied to a parameter, indicates that the parameter is an MVC action. If applied to a method, the MVC action name is calculated implicitly from the context. Use this
	///     attribute for custom wrappers similar to <c>System.Web.Mvc.Html.ChildActionExtensions.RenderAction(HtmlHelper, String)</c>
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcActionAttribute : Attribute
	{
		internal AspMvcActionAttribute() { }


		internal AspMvcActionAttribute([NotNull] string anonymousProperty) { this.AnonymousProperty = anonymousProperty; }


		[NotNull]
		internal string AnonymousProperty { get; private set; }
	}



	/// <summary>
	///     ASP.NET MVC attribute. Indicates that a parameter is an MVC area. Use this attribute for custom wrappers similar to
	///     <c>System.Web.Mvc.Html.ChildActionExtensions.RenderAction(HtmlHelper, String)</c>
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcAreaAttribute : PathReferenceAttribute
	{
		internal AspMvcAreaAttribute() { }


		internal AspMvcAreaAttribute([NotNull] string anonymousProperty) { this.AnonymousProperty = anonymousProperty; }


		[NotNull]
		internal string AnonymousProperty { get; private set; }
	}



	/// <summary>
	///     ASP.NET MVC attribute. If applied to a parameter, indicates that the parameter is an MVC controller. If applied to a method, the MVC controller name is calculated implicitly from the context. Use
	///     this attribute for custom wrappers similar to <c>System.Web.Mvc.Html.ChildActionExtensions.RenderAction(HtmlHelper, String, String)</c>
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcControllerAttribute : Attribute
	{
		internal AspMvcControllerAttribute() { }


		internal AspMvcControllerAttribute([NotNull] string anonymousProperty) { this.AnonymousProperty = anonymousProperty; }


		[NotNull]
		internal string AnonymousProperty { get; private set; }
	}



	/// <summary>
	///     ASP.NET MVC attribute. Indicates that a parameter is an MVC Master. Use this attribute for custom wrappers similar to <c>System.Web.Mvc.Controller.View(String, String)</c>
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcMasterAttribute : Attribute
	{
	}



	/// <summary>
	///     ASP.NET MVC attribute. Indicates that a parameter is an MVC model type. Use this attribute for custom wrappers similar to <c>System.Web.Mvc.Controller.View(String, Object)</c>
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcModelTypeAttribute : Attribute
	{
	}



	/// <summary>
	///     ASP.NET MVC attribute. If applied to a parameter, indicates that the parameter is an MVC partial view. If applied to a method, the MVC partial view name is calculated implicitly from the context.
	///     Use this attribute for custom wrappers similar to <c>System.Web.Mvc.Html.RenderPartialExtensions.RenderPartial(HtmlHelper, String)</c>
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcPartialViewAttribute : PathReferenceAttribute
	{
	}



	/// <summary>
	///     ASP.NET MVC attribute. Allows disabling all inspections for MVC views within a class or a method.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcSupressViewErrorAttribute : Attribute
	{
	}



	/// <summary>
	///     ASP.NET MVC attribute. Indicates that a parameter is an MVC display template. Use this attribute for custom wrappers similar to
	///     <c>System.Web.Mvc.Html.DisplayExtensions.DisplayForModel(HtmlHelper, String)</c>
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcDisplayTemplateAttribute : Attribute
	{
	}



	/// <summary>
	///     ASP.NET MVC attribute. Indicates that a parameter is an MVC editor template. Use this attribute for custom wrappers similar to
	///     <c>System.Web.Mvc.Html.EditorExtensions.EditorForModel(HtmlHelper, String)</c>
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcEditorTemplateAttribute : Attribute
	{
	}



	/// <summary>
	///     ASP.NET MVC attribute. Indicates that a parameter is an MVC template. Use this attribute for custom wrappers similar to <c>System.ComponentModel.DataAnnotations.UIHintAttribute(System.String)</c>
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcTemplateAttribute : Attribute
	{
	}



	/// <summary>
	///     ASP.NET MVC attribute. If applied to a parameter, indicates that the parameter is an MVC view. If applied to a method, the MVC view name is calculated implicitly from the context. Use this
	///     attribute for custom wrappers similar to <c>System.Web.Mvc.Controller.View(Object)</c>
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcViewAttribute : PathReferenceAttribute
	{
	}



	/// <summary>
	///     ASP.NET MVC attribute. When applied to a parameter of an attribute, indicates that this parameter is an MVC action name
	/// </summary>
	/// <example>
	///     <code>
	/// [ActionName("Foo")]
	/// internal ActionResult Login(string returnUrl) {
	///   ViewBag.ReturnUrl = Url.Action("Foo"); // OK
	///   return RedirectToAction("Bar"); // Error: Cannot resolve action
	/// }
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class AspMvcActionSelectorAttribute : Attribute
	{
	}



	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class HtmlElementAttributesAttribute : Attribute
	{
		internal HtmlElementAttributesAttribute() { }


		internal HtmlElementAttributesAttribute([NotNull] string name) { this.Name = name; }


		[NotNull]
		internal string Name { get; private set; }
	}



	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class HtmlAttributeValueAttribute : Attribute
	{
		internal HtmlAttributeValueAttribute([NotNull] string name) { this.Name = name; }


		[NotNull]
		internal string Name { get; private set; }
	}



	// Razor attributes

	/// <summary>
	///     Razor attribute. Indicates that a parameter or a method is a Razor section. Use this attribute for custom wrappers similar to <c>System.Web.WebPages.WebPageBase.RenderSection(String)</c>
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
	[GeneratedCode("Resharper", "8.1+")]
	public sealed class RazorSectionAttribute : Attribute
	{
	}


#pragma warning restore 1591
}