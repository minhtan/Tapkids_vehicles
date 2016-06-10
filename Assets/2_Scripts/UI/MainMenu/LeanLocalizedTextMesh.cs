using UnityEngine;
using System.Collections;

namespace Lean
{

	[ExecuteInEditMode]
	[RequireComponent(typeof(TextMesh))]
	[AddComponentMenu("Lean/Localized Text Mesh")]

	public class LeanLocalizedTextMesh : LeanLocalizedBehaviour {
		public bool AllowFallback;

		public string FallbackText;

		public int charSize;

		// This gets called every time the translation needs updating
		public override void UpdateTranslation(LeanTranslation translation)
		{
			// Get the Text component attached to this GameObject
			var text = GetComponent<TextMesh>();

			// Use translation?
			if (translation != null)
			{
				text.text = StringUltil.TextWrap(translation.Text, charSize);
			}
			// Use fallback?
			else if (AllowFallback == true)
			{
				text.text = FallbackText;
			}
		}

	}

}
