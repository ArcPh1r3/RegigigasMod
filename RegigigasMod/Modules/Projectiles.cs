using R2API;
using Rewired.ComponentControls.Effects;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.PostProcessing;

namespace RegigigasMod.Modules
{
    internal static class Projectiles
    {
        internal static GameObject earthPowerWave;

        internal static void RegisterProjectiles()
        {
            CreateEarthPowerWave();

            Modules.Prefabs.projectilePrefabs.Add(earthPowerWave);
        }

        private static void CreateEarthPowerWave()
        {
            earthPowerWave = CloneProjectilePrefab("BrotherUltLineProjectileRotateLeft", "EarthPowerProjectile");

            earthPowerWave.GetComponent<ProjectileDamage>().damageType = DamageType.IgniteOnHit;

            RegigigasPlugin.DestroyImmediate(earthPowerWave.GetComponent<RotateAroundAxis>());

            GameObject waveGhost = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/ProjectileGhosts/BrotherUltLineGhost"), "EarthPowerProjectileGhost", true);
            if (!waveGhost.GetComponent<NetworkIdentity>()) waveGhost.AddComponent<NetworkIdentity>();

            // gather materials and stuff
            PostProcessProfile magmaWormPP = Resources.Load<GameObject>("Prefabs/CharacterBodies/MagmaWormBody").GetComponentInChildren<PostProcessVolume>().sharedProfile;
            Material matMagmaOpaqueLarge = Resources.Load<GameObject>("Prefabs/ProjectileGhosts/MagmaOrbGhost").transform.Find("Particles").Find("SpitCore").GetComponent<ParticleSystemRenderer>().material;
            Material matMagmaOpaqueDirectional = Resources.Load<GameObject>("Prefabs/Effects/MagmaWormBurrow").transform.Find("ParticleLoop").Find("Magma, Directional").GetComponent<ParticleSystemRenderer>().material;
            Material titanPredictionEffect = Resources.Load<GameObject>("Prefabs/Projectiles/TitanPreFistProjectile").transform.Find("TeamAreaIndicator, GroundOnly").GetComponent<TeamAreaIndicator>().teamMaterialPairs[0].sharedMaterial;
            Material matSpiteBombPredictionEffect = Resources.Load<GameObject>("Prefabs/Effects/SpiteBombDelayEffect").transform.Find("Nova Sphere").GetComponent<ParticleSystemRenderer>().material;

            waveGhost.transform.Find("Size").Find("IndicatorFX").GetComponent<MeshRenderer>().material = titanPredictionEffect;
            waveGhost.transform.Find("Size").Find("IndicatorFX").Find("Edges").GetComponent<MeshRenderer>().material = matSpiteBombPredictionEffect;
            waveGhost.transform.Find("Size").Find("FireFX").Find("Dust, Directional").GetComponent<ParticleSystemRenderer>().material = matMagmaOpaqueDirectional;
            waveGhost.transform.Find("Size").Find("FireFX").Find("SparksUp").GetComponent<ParticleSystemRenderer>().material = matMagmaOpaqueLarge;

            waveGhost.transform.Find("Size").Find("FireFX").GetComponent<PostProcessVolume>().sharedProfile = magmaWormPP;

            earthPowerWave.GetComponent<ProjectileController>().ghostPrefab = waveGhost;
        }
        /*
        private static void CreateBomb()
        {
            bombPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            bombImpactExplosion.explosionSoundString = "HenryBombExplosion";
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombPrefab.GetComponent<ProjectileController>();
            bombController.ghostPrefab = CreateGhostPrefab("HenryBombGhost");
            bombController.startSound = "";
        }
        */
        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.explosionSoundString = "";
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeExpiredSoundString = "";
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            ghostPrefab.AddComponent<NetworkIdentity>();
            ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}