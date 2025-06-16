using Microsoft.Phone.Shell;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Windows.Storage;

namespace ScheduledAgent
{
	internal class FlipTemplateLog
	{
		private const string DefaultFolder = "Assets/Tiles/";

		private const string DefaultExt = ".png";

		private const string DefaultSmallImage = "SmallTile";

		private const string DefaultImage = "MediumTile";

		private const string DefaultBackImage = "BackMediumTile";

		private const string DefaultWideImage = "LargeTile";

		private const string DefaultWideBackImage = "BackLargeTile";

		private const int NumberOfBackImages = 4;

		private string LogFileName;

		private FlipTileData m_flipTileData;

		private int m_curBackImage;

		private int m_curWideBackImage;

		public FlipTemplateLog()
		{
			this.m_flipTileData = new FlipTileData();
			string str = "Assets/Tiles/SmallTile.png";
			if (File.Exists(str))
			{
				this.m_flipTileData.set_SmallBackgroundImage(new Uri(string.Concat("/", str), UriKind.Relative));
			}
			str = "Assets/Tiles/MediumTile.png";
			if (File.Exists(str))
			{
				this.m_flipTileData.set_BackgroundImage(new Uri(string.Concat("/", str), UriKind.Relative));
			}
			str = "Assets/Tiles/BackMediumTile.png";
			if (File.Exists(str))
			{
				this.m_flipTileData.set_BackBackgroundImage(new Uri(string.Concat("/", str), UriKind.Relative));
			}
			str = "Assets/Tiles/LargeTile.png";
			if (File.Exists(str))
			{
				this.m_flipTileData.set_WideBackgroundImage(new Uri(string.Concat("/", str), UriKind.Relative));
			}
			str = "Assets/Tiles/BackLargeTile.png";
			if (File.Exists(str))
			{
				this.m_flipTileData.set_WideBackBackgroundImage(new Uri(string.Concat("/", str), UriKind.Relative));
			}
			this.LogFileName = string.Concat(ApplicationData.Current.LocalFolder.Path, "\\FlipTemplate.log");
			this.m_curBackImage = 0;
			this.m_curWideBackImage = 0;
		}

		public int Read()
		{
			Random random = new Random();
			this.m_curBackImage = random.Next(0, 4);
			this.m_curWideBackImage = random.Next(0, 4);
			return 0;
		}

		public int Update()
		{
			int num = 0;
			num = this.Read();
			num = this.UpdateImage();
			num = this.UpdateBackImage(false);
			return this.Write();
		}

		public int UpdateBackImage(bool reset)
		{
			int num = 0;
			string str = null;
			ShellTile shellTile = ShellTile.get_ActiveTiles().First<ShellTile>();
			if (reset)
			{
				this.m_curBackImage = 0;
				str = "Assets/Tiles/BackMediumTile.png";
				this.m_flipTileData.set_BackBackgroundImage(new Uri(string.Concat("/", str), UriKind.Relative));
				this.m_curWideBackImage = 0;
				str = "Assets/Tiles/BackLargeTile.png";
				this.m_flipTileData.set_WideBackBackgroundImage(new Uri(string.Concat("/", str), UriKind.Relative));
				shellTile.Update(this.m_flipTileData);
				num = 10;
				return num;
			}
			this.m_curBackImage++;
			str = string.Concat("Assets/Tiles/BackMediumTile", this.m_curBackImage, ".png");
			if (!File.Exists(str))
			{
				this.m_curBackImage = 0;
				str = "Assets/Tiles/BackMediumTile.png";
				this.m_flipTileData.set_BackBackgroundImage(new Uri(string.Concat("/", str), UriKind.Relative));
				num++;
			}
			else
			{
				this.m_flipTileData.set_BackBackgroundImage(new Uri(string.Concat("/", str), UriKind.Relative));
			}
			this.m_curWideBackImage++;
			str = string.Concat("Assets/Tiles/BackLargeTile", this.m_curWideBackImage, ".png");
			if (!File.Exists(str))
			{
				this.m_curWideBackImage = 0;
				str = "Assets/Tiles/BackLargeTile.png";
				this.m_flipTileData.set_WideBackBackgroundImage(new Uri(string.Concat("/", str), UriKind.Relative));
				num += 2;
			}
			else
			{
				this.m_flipTileData.set_WideBackBackgroundImage(new Uri(string.Concat("/", str), UriKind.Relative));
			}
			shellTile.Update(this.m_flipTileData);
			return num;
		}

		public int UpdateImage()
		{
			string str = null;
			string englishName = CultureInfo.CurrentUICulture.EnglishName;
			if (englishName.Contains("Chinese") && englishName.Contains("Simplified"))
			{
				str = "_CH";
			}
			if (str == null)
			{
				return 0;
			}
			string str1 = string.Concat("Assets/Tiles/LargeTile", str, ".png");
			this.m_flipTileData.set_WideBackgroundImage(new Uri(string.Concat("/", str1), UriKind.Relative));
			return 1;
		}

		public int Write()
		{
			return 0;
		}
	}
}