namespace Archeology.Texture
{
	public static class ModTexture {
		public const string CommonTextureStringPattern = "Archeology/Texture/";
		public const string MissingTexture_Folder = "MissingTexture/";
		public const string PinIcon = CommonTextureStringPattern + "UI/PinIcon";

		public const string WHITEDOT = "Archeology/Texture/WhiteDot";
		public const string MISSINGTEXTUREPOTION = "Archeology/Texture/MissingTexturePotion";
		public const string EMPTYBUFF = "Archeology/Texture/EmptyBuff";
		public const string WHITEBALL = "Archeology/Texture/WhiteBall";
		public const string DIAMONDSWOTAFFORB = "Archeology/Texture/DiamondSwotaffOrb";
		public const string ACCESSORIESSLOT = "Terraria/Images/Inventory_Back7";
		public const string MENU = "Archeology/Texture/UI/menu";
		public const string SMALLWHITEBALL = "v/Texture/smallwhiteball";
		public const string Lock = "Archeology/Texture/UI/lock";
		public const string Arrow_Left = CommonTextureStringPattern + "UI/LeftArrow";
		public const string Arrow_Right = CommonTextureStringPattern + "UI/RightArrow";
		public const string OuterInnerGlow = CommonTextureStringPattern + "OuterInnerGlow";
		/// <summary>
		/// Width : 16 | Height : 16
		/// </summary>
		public const string Boxes = CommonTextureStringPattern + "UI/Boxes";
		public const string QuestionMark_Help = CommonTextureStringPattern + "UI/Help";
		public const string Page_StateSelected = CommonTextureStringPattern + "UI/page_selected";
		public const string Page_StateUnselected = CommonTextureStringPattern + "UI/page_unselected";
		public static string Get_MissingTexture(string text) => CommonTextureStringPattern + MissingTexture_Folder + $"{text}MissingTexture";
		public const string MissingTexture_Default = CommonTextureStringPattern + MissingTexture_Folder + "MissingTextureDefault";
	}
}
