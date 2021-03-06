﻿using OpenClassic.Server.Collections;
using System;

namespace OpenClassic.Server.Domain
{
    public interface IPlayer : IEquatable<IPlayer>, IIndexable, ILocatable
    {
        new bool Active { get; set; }

        #region Appearance fields

        byte HairColour { get; set; }
        byte TopColour { get; set; }
        byte TrouserColour { get; set; }
        byte SkinColour { get; set; }

        int Head { get; set; }
        int Body { get; set; }
        bool Male { get; set; }
        bool AppearanceUpdateRequired { get; set; }

        int GetSprite(int pos);

        int[] GetSprites();

        #endregion

        #region World view fields

        EntityCollection<IPlayer> WatchedPlayers { get; }
        EntityCollection<INpc> WatchedNpcs { get; }
        EntityCollection<IGameObject> WatchedObjects { get; }

        void RevalidateWatchedNpcs();
        void RevalidateWatchedObjects();
        void RevalidateWatchedPlayers();

        void UpdateWatchedNpcs();
        void UpdateWatchedObjects();
        void UpdateWatchedPlayers();

        #endregion

        #region Movement fields


        void UpdatePosition();
        void SetPath(Path path);
        void ResetPath();

        #endregion
    }
}
