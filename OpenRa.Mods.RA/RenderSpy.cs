﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenRa.Graphics;
using OpenRa.Traits;

namespace OpenRa.Mods.RA
{
	class RenderSpyInfo : RenderInfantryInfo
	{
		public override object Create(Actor self) { return new RenderSpy(self); }
	}

	class RenderSpy : RenderInfantry, IRenderModifier
	{
		public RenderSpy(Actor self)
			: base(self)
		{
		}

		public IEnumerable<Renderable> ModifyRender(Actor self, IEnumerable<Renderable> r)
		{
			if (self.Owner == self.World.LocalPlayer)
				return r;

			return r.Select(a => a.WithPalette(self.World.LocalPlayer.Palette));
		}

		public override void Tick(Actor self)
		{
			anim.ChangeImage(self.Owner == self.World.LocalPlayer ? GetImage(self) : "e1");
			base.Tick(self);
		}
	}
}
