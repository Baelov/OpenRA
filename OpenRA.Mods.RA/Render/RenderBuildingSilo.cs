﻿#region Copyright & License Information
/*
 * Copyright 2007-2011 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using OpenRA.Traits;

namespace OpenRA.Mods.RA.Render
{
	class RenderBuildingSiloInfo : RenderBuildingInfo
	{
		public override object Create(ActorInitializer init) { return new RenderBuildingSilo(init, this); }
	}

	class RenderBuildingSilo : RenderBuilding, INotifyBuildComplete, INotifyCapture
	{
		PlayerResources playerResources;

		public RenderBuildingSilo( ActorInitializer init, RenderBuildingSiloInfo info )
			: base(init, info)
		{
			playerResources = init.self.Owner.PlayerActor.Trait<PlayerResources>();
		}

		public void BuildingComplete(Actor self)
		{
			var animation = (self.GetDamageState() >= DamageState.Heavy) ? "damaged-idle" : "idle";
			anim.PlayFetchIndex(animation,
				() => playerResources.OreCapacity != 0
					? ((10 * anim.CurrentSequence.Length - 1) * playerResources.Ore) / (10 * playerResources.OreCapacity)
					: 0);
		}

		public void OnCapture (Actor self, Actor captor, Player oldOwner, Player newOwner)
		{
			playerResources = newOwner.PlayerActor.Trait<PlayerResources>();
		}
	}
}
