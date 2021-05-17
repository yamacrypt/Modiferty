using System;
using Hexat;
using UnityEngine;
namespace MackySoft.Modiferty {

	public interface IReadOnlyModifiableProperty<T> {

		/// <summary>
		/// Base value before the evaluation.
		/// </summary>
		T BaseValue { get; }

		/// <summary>
		/// List of <see cref="IModifier{T}"/> to evaluate base value.
		/// </summary>
		IReadOnlyModifierList<T> Modifiers { get; }

		/// <summary>
		/// <para> Whether has modifiers. </para> 
		/// <para> This is used to avoid creating unnecessary ModifierList. </para>
		/// </summary>
		bool HasModifiers { get; }

		/// <summary>
		/// Evaluate the base value by modifiers.
		/// </summary>
		T Evaluate ();

	}

	public interface IModifiableProperty<T> : IReadOnlyModifiableProperty<T> {

		/// <summary>
		/// Base value before the evaluation.
		/// </summary>
		new T BaseValue { get; set; }

		/// <summary>
		/// List of <see cref="IModifier{T}"/> to evaluate base value.
		/// </summary>
		new IModifierList<T> Modifiers { get; }

	}
    public class SecureModifieableProperty<T> : IModifiableProperty<T>
    {
        [SerializeField]
		ISecureValue<T> m_BaseValue= (ISecureValue<T>)SecureValue.New<T>();

		[NonSerialized]
		ModifierList<T> m_Modifiers;

		public T BaseValue {
			get => m_BaseValue.Value;
			set => m_BaseValue.Value=value;
		}

		public IModifierList<T> Modifiers => m_Modifiers ?? (m_Modifiers = new ModifierList<T>());

		public bool HasModifiers => (m_Modifiers != null) && (m_Modifiers.Count > 0);

		IReadOnlyModifierList<T> IReadOnlyModifiableProperty<T>.Modifiers => Modifiers;
		
		public SecureModifieableProperty () : this(default) {
		}

		public SecureModifieableProperty (T baseValue) {
			m_BaseValue.Value = baseValue;
		}

		public T Evaluate () {
			return (m_Modifiers != null) ? m_Modifiers.Evaluate(m_BaseValue.Value) : m_BaseValue.Value;
		}
    }
	
	
    

}
