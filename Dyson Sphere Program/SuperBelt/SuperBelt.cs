using BepInEx;
using xiaoye97;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace SuperBelt
{
    [BepInDependency("me.xiaoye97.plugin.Dyson.LDBTool", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("me.xiaoye97.plugin.Dyson.SuperBelt", "SuperBelt", "1.1")]
    public class SuperBelt : BaseUnityPlugin
    {
        Sprite belt4Icon;
        void Start()
        {
            LDBTool.PreAddDataAction += AddTranslate;
            LDBTool.PostAddDataAction += AddBeltData;
            var ab = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("SuperBelt.belt"));
            belt4Icon = ab.LoadAsset<Sprite>("belt-4");
        }

        void AddTranslate()
        {
            // 添加翻译
            StringProto nameString = new StringProto();
            nameString.ID = 10001;
            nameString.Name = "SuperBelt_Name";
            nameString.name = "SuperBelt_Name";
            nameString.ZHCN = "超级传送带";
            nameString.ENUS = "Super Belt";
            nameString.FRFR = "Super Belt";
            StringProto descString = new StringProto();
            descString.ID = 10002;
            descString.Name = "SuperBelt_Desc";
            descString.name = "SuperBelt_Desc";
            descString.ZHCN = "比急速传送带更强力的设备，有效升级你的工厂！它可以60/s传输物品!";
            descString.ENUS = "It can transmit items at 60/s!";
            descString.FRFR = "It can transmit items at 60/s!";
            LDBTool.PreAddProto(ProtoType.String, nameString);
            LDBTool.PreAddProto(ProtoType.String, descString);
        }

        void AddBeltData()
        {
            // 因为部分数据可以复用，所以直接从现有库中读取
            var belt3 = LDB.items.Select(2003);
            var belt3r = LDB.recipes.Select(92);
            var preTech = LDB.techs.Select(1605);

            ItemProto belt4 = new ItemProto();
            RecipeProto belt4r = new RecipeProto();
            // 设置图标
            Traverse.Create(belt4).Field("_iconSprite").SetValue(belt4Icon);
            Traverse.Create(belt4r).Field("_iconSprite").SetValue(belt4Icon);
            // 设置合成配方数据
            belt4r.ID = 201;
            belt4r.Name = "SuperBelt_Name";
            belt4r.name = "SuperBelt_Name".Translate();
            belt4r.Description = "SuperBelt_Desc";
            belt4r.description = "SuperBelt_Desc".Translate();
            belt4r.TimeSpend = belt3r.TimeSpend;
            belt4r.Items = new int[] { 2003, 1205, 1124 }; // 合成材料
            belt4r.ItemCounts = new int[] { 3, 1, 1 }; // 合成材料需要的数量
            belt4r.Results = new int[] { 2004 }; // 合成结果
            belt4r.ResultCounts = new int[] { 3 }; // 合成结果的数量
            belt4r.GridIndex = 2501; // 在合成表中的位置，第2页，第5排，第1个
            belt4r.preTech = preTech;
            belt4r.SID = belt4r.GridIndex.ToString();
            belt4r.sid = belt4r.GridIndex.ToString();
            belt4r.Type = ERecipeType.Assemble; // 合成方式为制造台
            belt4r.Handcraft = true; // 可以手动合成

            // 设置物品数据
            belt4.Name = "SuperBelt_Name";
            belt4.name = "SuperBelt_Name".Translate();
            belt4.Description = "SuperBelt_Desc";
            belt4.description = "SuperBelt_Desc".Translate();
            belt4.ID = 2004;
            belt4.Type = belt3.Type; // 物品类型
            belt4.StackSize = belt3.StackSize; // 物品堆叠
            belt4.IsEntity = belt3.IsEntity;
            belt4.CanBuild = belt3.CanBuild;
            belt4.makes = new List<RecipeProto>();
            belt4.ModelIndex = belt3.ModelIndex; // 模型序号，沿用急速传送带
            belt4.ModelCount = belt3.ModelCount;
            belt4.HpMax = belt3.HpMax;
            belt4.BuildIndex = 351; // 不要和现有序号重复
            belt4.BuildMode = belt3.BuildMode;
            belt4.GridIndex = belt4r.GridIndex;
            belt4.DescFields = new int[] { 15, 1 };
            belt4.handcraft = belt4r;
            belt4.maincraft = belt4r;
            belt4.handcraftProductCount = 3;
            belt4.maincraftProductCount = 3;
            belt4.handcrafts = new List<RecipeProto>() { belt4r };
            belt4.rawMats = belt3.rawMats;
            belt4.isRaw = belt3.isRaw;
            belt4.recipes = new List<RecipeProto>() { belt4r }; // 设置有哪些配方可以合成此物品(用于UI显示)
            belt4.prefabDesc = new PrefabDesc();
            belt4.prefabDesc.modelIndex = belt4.ModelIndex;
            belt4.prefabDesc.hasObject = true;
            belt4.prefabDesc.prefab = belt3.prefabDesc.prefab;
            belt4.prefabDesc.colliderPrefab = belt3.prefabDesc.colliderPrefab;
            belt4.prefabDesc.startInstCapacity = belt3.prefabDesc.startInstCapacity;
            belt4.prefabDesc.batchCapacity = belt3.prefabDesc.batchCapacity;
            belt4.prefabDesc.colliders = belt3.prefabDesc.colliders;
            belt4.prefabDesc.buildCollider = belt3.prefabDesc.buildCollider;
            belt4.prefabDesc.buildColliders = belt3.prefabDesc.buildColliders;
            belt4.prefabDesc.isBelt = true;
            belt4.prefabDesc.beltSpeed = 10;
            belt4.prefabDesc.beltPrototype = 2004;
            belt4.prefabDesc.anim_working_length = belt3.prefabDesc.anim_working_length;
            belt4.prefabDesc.selectAlpha = belt3.prefabDesc.selectAlpha;
            belt4.prefabDesc.selectDistance = belt3.prefabDesc.selectDistance;
            belt4.prefabDesc.signHeight = belt3.prefabDesc.signHeight;
            belt4.prefabDesc.signSize = belt3.prefabDesc.signSize;
            LDBTool.PostAddProto(ProtoType.Recipe, belt4r);
            LDBTool.PostAddProto(ProtoType.Item, belt4);
        }
    }
}
