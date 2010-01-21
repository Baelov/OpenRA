﻿using System.Collections.Generic;
using System.Linq;
using OpenRa.Effects;
using OpenRa.Traits;

namespace OpenRa.Mods.RA
{
	class MineInfo : ITraitInfo
	{
		public readonly int Damage = 0;
		public readonly UnitMovementType[] TriggeredBy = { };
		public readonly string Warhead = "ATMine";
		public readonly bool AvoidFriendly = true;

		public object Create(Actor self) { return new Mine(self); }
	}

	class Mine : ICrushable, IOccupySpace
	{
		readonly Actor self;
		public Mine(Actor self)
		{
			this.self = self;
			self.World.UnitInfluence.Add(self, this);
		}

		public void OnCrush(Actor crusher)
		{
			if (crusher.traits.Contains<MineImmune>() && crusher.Owner == self.Owner)
				return;

			var info = self.Info.Traits.Get<MineInfo>();
			var warhead = Rules.WarheadInfo[info.Warhead];

			self.World.AddFrameEndTask(_ =>
			{
				self.World.Remove(self);
				self.World.Add(new Explosion(self.CenterLocation.ToInt2(), warhead.Explosion, false));
				crusher.InflictDamage(crusher, info.Damage, warhead);
			});
		}

		public bool IsPathableCrush(UnitMovementType umt, Player player)
		{
			return !self.Info.Traits.Get<MineInfo>().AvoidFriendly || (player != self.World.LocalPlayer);
		}

		public bool IsCrushableBy(UnitMovementType umt, Player player)
		{
			return self.Info.Traits.Get<MineInfo>().TriggeredBy.Contains(umt);
		}

		public IEnumerable<int2> OccupiedCells() { yield return self.Location; }
	}
}
