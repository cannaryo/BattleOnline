using System;

namespace NetgameProj
{
	/// <summary>
	/// �萔�̒�`
	/// </summary>
	public struct Defines
	{
		public static readonly Version version = new Version(1,1);	//�o�[�W�����Ǘ�
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
			"��D�̃J�[�h��I��ł��������B",
			"�t���b�O�܂��́u�g�p�v��I��ł��������B",
			"�g���[�v�܂��̓^�N�e�B�N�X�J�[�h�������Ă��������B",
			"(��@)�g���[�v�܂��̓^�N�e�B�N�X�J�[�h�������Ă��������B",
			"(��@)�P���ڂ̖߂��J�[�h��I��ł��������B",
			"(��@)�Q���ڂ̖߂��J�[�h��I��ł��������B",
			"(�E��)�G�̃g���[�v�܂��̓^�N�e�B�N�X�J�[�h��I��ł��������B",
			"(���؂�)�G�̃g���[�v�J�[�h��I��ł��������B",
			"(���؂�)�Ĕz�u���I��ł��������B",
			"(�]�i)�ړ������鎩�R�̃g���[�v���^�N�e�B�N�X�J�[�h��I��ł��������B",
			"(�]�i)�Ĕz�u��܂��́u�p���v��I��ł��������B",
            "���̃Q�[�����n�߂�ɂ́A�uReset�v��I��ł��������B"
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
		public static readonly string [] CaptionColor = { "��","��","��","��","��","��"};
		public static readonly string [] CaptionNumber = { "�O","�P","�Q","�R","�S","�T","�U","�V","�W","�X","�P�O"};
		public static readonly string [] CaptionTactics =
		{
			"�^�N�e�B�N�X�J�[�h�F�D�^\n���̃t���b�O�̗��R�Ƃ����`�ɂS���̃g���[�v��K�v�Ƃ���",
			"�^�N�e�B�N�X�J�[�h�F�Z��\n���̃t���b�O�̗��R�Ƃ����`�̋��x�������ƂȂ�\n�g���[�v�R���̍��v�l�Ńt���b�O�l���������s��",
			"�^�N�e�B�N�X�J�[�h�F��@\n���v�R���̃J�[�h�������ꂩ�̎R�D��������A��D�Ƃ���\n���̌�A��D����Q�����R�D�̏�ɖ߂�",
			"�^�N�e�B�N�X�J�[�h�F���؂�\n�������t���b�O�̓G�R�̔C�ӂ̃g���[�v���ꖇ���R�ɍĔz�u����",
			"�^�N�e�B�N�X�J�[�h�F�]�i\n�������t���b�O�̎��R�̔C�ӂ̃g���[�v���邢�̓^�N�e�B�N�X��I������\n��������R�̔C�ӂ̃v���C�\�ȃt���b�O�ɍĔz�u���邩�A�j������",
			"�^�N�e�B�N�X�J�[�h�F�E��\n�������t���b�O�̓G�R�̔C�ӂ̃g���[�v���邢�̓^�N�e�B�N�X��j������",
			"�^�N�e�B�N�X�J�[�h�F�~������\n�g���[�v�̂悤�Ƀv���C�ł��邪�A�F�Ɛ����i�P�`�R�j�̓t���b�O�l���������Ɍ��肷��",
			"�^�N�e�B�N�X�J�[�h�F�����R��\n���l�W�̃g���[�v�Ƃ��Č���ł��邪�A�F�̓t���b�O�l���������Ɍ��肷��",
			"�^�N�e�B�N�X�J�[�h�F�_���C�I�X\n�g���[�v�̂悤�Ƀv���C�ł��邪�A�F�Ɛ����̓t���b�O�l���������Ɍ��肷��\n�i���[�_�[�͎��R�Ɉꖇ�����v���C�ł��Ȃ��j",
			"�^�N�e�B�N�X�J�[�h�F�A���L�T���_�[\n�g���[�v�̂悤�Ƀv���C�ł��邪�A�F�Ɛ����̓t���b�O�l���������Ɍ��肷��\n�i���[�_�[�͎��R�Ɉꖇ�����v���C�ł��Ȃ��j"
		};
        public static readonly string CaptionFlag = "�Ԗڂ̃t���b�O�B\n�g���[�v�J�[�h��u�����Ƃ��ł���B\n�t���b�O�l���������s���ɂ́A�h���[�t�F�C�Y����D�I���t�F�C�Y�ɑI������B";
        public static readonly string CaptionWin = "���߂łƂ��I�Q�[���ɏ������܂����B";
        public static readonly string CaptionLose = "�c�O�Ȃ���A�Q�[���ɔs�k���܂����B";
    }
}