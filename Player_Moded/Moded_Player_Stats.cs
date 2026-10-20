using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;

namespace Content.Player_Moded;


public partial class Moded_Player_Stats : ModPlayer
{
    private PlayerState playerState = PlayerState.Idle;
    private Subclasses subclass = Subclasses.None;

    private Dictionary<Subclasses, float> subclassStats = new();

    // Barbarian subclass stats
    private float max_rage = 100f;
    private float current_rage = 0f;
    private float rageAddedOnHit = 10f;
    private const float rageLostPerSecondPercent = 2f;


    // Barbarian subclass methods
    public void addRage(float amount)
    {

        this.current_rage += amount;
        if (this.current_rage >= this.max_rage){
        
            this.current_rage = this.max_rage;
        }
    }

    public void reduceRage(float amount)
    {
        this.current_rage -= amount;
        if (this.current_rage <= 0f){
        
            this.current_rage = 0f;
        }
    }



    // General methods and Getters / Setters for subclasses
    public float getRageAddedOnHit()
    {
        return rageAddedOnHit;
    }

    public void setRageAddedOnHit(float value)
    {
        rageAddedOnHit = value;
    }


    public override void PostUpdateEquips()
    {
        Player.GetDamage(DamageClass.Melee) += current_rage / 100f; // Increase melee damage based on current rage
    }

    public override void PostUpdate()
    {
        if (subclass == Subclasses.Barbarian)
        {
            reduceRage(this.current_rage * rageLostPerSecondPercent / 100f / 60f);
        }
    }

    public override void OnHurt(Player.HurtInfo info)
    {
        if (subclass == Subclasses.Barbarian)
        {
            reduceRage(this.current_rage * 0.1f); // Reduce rage by 10% when the player is hurt
        }
    }


    public void setSubclass(Subclasses subclass)
    {
        this.subclass = subclass;
    } 

    public void setPlayerState(PlayerState playerState)
    {
        this.playerState = playerState;
    }

    public Subclasses getSubclass()
    {
        return subclass;
    }


    


    public PlayerState getPlayerState()
    {
        return playerState;
    }


    public void setSubclassStat(Subclasses subclass, float stat)
    {
        subclassStats[subclass] = stat;
    }

    public void addSubclassStatValue(Subclasses subclass, float value)
    {
        if (subclassStats.ContainsKey(subclass))
        {
            subclassStats[subclass] += value;
        }
        else
        {
            subclassStats[subclass] = value;
        }
    }

    public float getSubclassStat(Subclasses subclass)
    {
        if (subclassStats.TryGetValue(subclass, out float stat))
        {
            return stat;
        }
        else
        {
            return 0f;
        }
    }





    public override void ResetEffects()
    {
        subclass = Subclasses.None;

        foreach (Subclasses key in subclassStats.Keys.ToList())
        {
            subclassStats[key] = 0f;
        }
            
        

    }

}   