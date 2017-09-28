using System;

namespace NetgameProj
{
	/// <summary>
	/// 定数の定義
	/// </summary>
	public struct Defines
	{
		public static readonly Version version = new Version(1,1);	//バージョン管理
		public const string FileCardImage = @"data/card.bmp";
		public const string FileTacticsImage = @"data/tactics.bmp";
		public const string FileEtcImage = @"data/etc.bmp";
		public const int CardSizeX = 64;
		public const int CardSizeY = 64;
		public const int MaxHand = 7;
		public const int NumOfFlag = 9;
		public const int NumberOfTactics = 10;
        public const int CountForWin = 5;
        public const int SequencedCountForWin = 3;
        public struct ImageName
		{
			public const string BlackTCard		= "BlackTCard";
			public const string BlackCard		= "BlackCard";
			public const string TacticsStack	= "TacticsStack";
			public const string TroopStack		= "TroopStack";
			public const string Flag			= "Flag";
			public const string LabelUse		= "LabelUse";
			public const string LabelDestroy	= "LabelDestroy";
			public const string LabelReset		= "LabelReset";
			public const string LabelNext		= "LabelNext";
			public const string LabelWin		= "LabelWin";
			public const string LabelLose		= "LabelLose";
			public const string SelectBox		= "SelectBox";
			public const string EffFog			= "EffFog";
			public const string EffMud			= "EffMud";
		};
		public enum Tactics 
		{
			MUD,
			FOG,
			SCOUT,
			TRAITOR,
			REDEPLOY,
			DESERTER,
			SHIELD_BEARERS,
			COMPANION_CAVAERY,
			DARIUS,
			ALEXANDER,
			TROOP
		};
		public static readonly string [] CaptionPhase = 
		{
			"手札のカードを選んでください。",
			"フラッグまたは「使用」を選んでください。",
			"トループまたはタクティクスカードを引いてください。",
			"(偵察)トループまたはタクティクスカードを引いてください。",
			"(偵察)１枚目の戻すカードを選んでください。",
			"(偵察)２枚目の戻すカードを選んでください。",
			"(脱走)敵のトループまたはタクティクスカードを選んでください。",
			"(裏切り)敵のトループカードを選んでください。",
			"(裏切り)再配置先を選んでください。",
			"(転進)移動させる自軍のトループかタクティクスカードを選んでください。",
			"(転進)再配置先または「廃棄」を選んでください。",
            "次のゲームを始めるには、「Reset」を選んでください。"
        };
		public enum Phase
		{
			ChooseCard,
			ChooseFlag,
			ChooseStack,
			ScoutDraw,
			Discard1,
			Discard2,
			Deserter,
			Traitor1,
			Traitor2,
			Redeploy1,
			Redeploy2,
            FinishGame
		};
		public enum StackType
		{
			Troop,
			Tactics
		};
		public static readonly string [] CaptionColor = { "赤","緑","黄","青","紫","水"};
		public static readonly string [] CaptionNumber = { "０","１","２","３","４","５","６","７","８","９","１０"};
		public static readonly string [] CaptionTactics =
		{
			"タクティクスカード：泥濘\nこのフラッグの両軍とも隊形に４枚のトループを必要とする",
			"タクティクスカード：濃霧\nこのフラッグの両軍とも隊形の強度が無効となる\nトループ３枚の合計値でフラッグ獲得解決を行う",
			"タクティクスカード：偵察\n合計３枚のカードをいずれかの山札から引き、手札とする\nその後、手札から２枚を山札の上に戻す",
			"タクティクスカード：裏切り\n未解決フラッグの敵軍の任意のトループを一枚自軍に再配置する",
			"タクティクスカード：転進\n未解決フラッグの自軍の任意のトループあるいはタクティクスを選択する\nそれを自軍の任意のプレイ可能なフラッグに再配置するか、破棄する",
			"タクティクスカード：脱走\n未解決フラッグの敵軍の任意のトループあるいはタクティクスを破棄する",
			"タクティクスカード：円盾部隊\nトループのようにプレイできるが、色と数字（１～３）はフラッグ獲得解決時に決定する",
			"タクティクスカード：随伴騎兵\n数値８のトループとして決定できるが、色はフラッグ獲得解決時に決定する",
			"タクティクスカード：ダレイオス\nトループのようにプレイできるが、色と数字はフラッグ獲得解決時に決定する\n（リーダーは自軍に一枚しかプレイできない）",
			"タクティクスカード：アレキサンダー\nトループのようにプレイできるが、色と数字はフラッグ獲得解決時に決定する\n（リーダーは自軍に一枚しかプレイできない）"
		};
        public static readonly string CaptionFlag = "番目のフラッグ。\nトループカードを置くことができる。\nフラッグ獲得解決を行うには、ドローフェイズか手札選択フェイズに選択する。";
        public static readonly string CaptionWin = "おめでとう！ゲームに勝利しました。";
        public static readonly string CaptionLose = "残念ながら、ゲームに敗北しました。";
    }
}