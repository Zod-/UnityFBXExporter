using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxNodeTemplateNode : FbxPropertyTemplateBaseNode
    {
        public FbxNodeTemplateNode() : base("FbxNode")
        {
            Property("QuaternionInterpolate", "enum", "", "", 0);
            Property("RotationOffset", "Vector3D", "Vector", "", Vector3.zero);
            Property("RotationPivot", "Vector3D", "Vector", "", Vector3.zero);
            Property("ScalingOffset", "Vector3D", "Vector", "", Vector3.zero);
            Property("ScalingPivot", "Vector3D", "Vector", "", Vector3.zero);
            Property("TranslationActive", "bool", "", "", 0);
            Property("TranslationMin", "Vector3D", "Vector", "", Vector3.zero);
            Property("TranslationMax", "Vector3D", "Vector", "", Vector3.zero);
            Property("TranslationMinX", "bool", "", "", 0);
            Property("TranslationMinY", "bool", "", "", 0);
            Property("TranslationMinZ", "bool", "", "", 0);
            Property("TranslationMaxX", "bool", "", "", 0);
            Property("TranslationMaxY", "bool", "", "", 0);
            Property("TranslationMaxZ", "bool", "", "", 0);
            Property("RotationOrder", "enum", "", "", 0);
            Property("RotationSpaceForLimitOnly", "bool", "", "", 0);
            Property("RotationStiffnessX", "double", "Number", "", 0);
            Property("RotationStiffnessY", "double", "Number", "", 0);
            Property("RotationStiffnessZ", "double", "Number", "", 0);
            Property("AxisLen", "double", "Number", "", 10);
            Property("PreRotation", "Vector3D", "Vector", "", Vector3.zero);
            Property("PostRotation", "Vector3D", "Vector", "", Vector3.zero);
            Property("RotationActive", "bool", "", "", 0);
            Property("RotationMin", "Vector3D", "Vector", "", Vector3.zero);
            Property("RotationMax", "Vector3D", "Vector", "", Vector3.zero);
            Property("RotationMinX", "bool", "", "", 0);
            Property("RotationMinY", "bool", "", "", 0);
            Property("RotationMinZ", "bool", "", "", 0);
            Property("RotationMaxX", "bool", "", "", 0);
            Property("RotationMaxY", "bool", "", "", 0);
            Property("RotationMaxZ", "bool", "", "", 0);
            Property("InheritType", "enum", "", "", 0);
            Property("ScalingActive", "bool", "", "", 0);
            Property("ScalingMin", "Vector3D", "Vector", "", Vector3.zero);
            Property("ScalingMax", "Vector3D", "Vector", "", Vector3.one);
            Property("ScalingMinX", "bool", "", "", 0);
            Property("ScalingMinY", "bool", "", "", 0);
            Property("ScalingMinZ", "bool", "", "", 0);
            Property("ScalingMaxX", "bool", "", "", 0);
            Property("ScalingMaxY", "bool", "", "", 0);
            Property("ScalingMaxZ", "bool", "", "", 0);
            Property("GeometricTranslation", "Vector3D", "Vector", "", Vector3.zero);
            Property("GeometricRotation", "Vector3D", "Vector", "", Vector3.zero);
            Property("GeometricScaling", "Vector3D", "Vector", "", Vector3.one);
            Property("MinDampRangeX", "double", "Number", "", 0);
            Property("MinDampRangeY", "double", "Number", "", 0);
            Property("MinDampRangeZ", "double", "Number", "", 0);
            Property("MaxDampRangeX", "double", "Number", "", 0);
            Property("MaxDampRangeY", "double", "Number", "", 0);
            Property("MaxDampRangeZ", "double", "Number", "", 0);
            Property("MinDampStrengthX", "double", "Number", "", 0);
            Property("MinDampStrengthY", "double", "Number", "", 0);
            Property("MinDampStrengthZ", "double", "Number", "", 0);
            Property("MaxDampStrengthX", "double", "Number", "", 0);
            Property("MaxDampStrengthY", "double", "Number", "", 0);
            Property("MaxDampStrengthZ", "double", "Number", "", 0);
            Property("PreferedAngleX", "double", "Number", "", 0);
            Property("PreferedAngleY", "double", "Number", "", 0);
            Property("PreferedAngleZ", "double", "Number", "", 0);
            Property("LookAtProperty", "object", "", "", ' ');
            Property("UpVectorProperty", "object", "", "", ' ');
            Property("Show", "bool", "", "", 1);
            Property("NegativePercentShapeSupport", "bool", "", "", 1);
            Property("DefaultAttributeIndex", "int", "Integer", "", -1);
            Property("Freeze", "bool", "", "", 0);
            Property("LODBox", "bool", "", "", 0);
            Property("Lcl Translation", "Lcl Translation", "", "A", Vector3.zero);
            Property("Lcl Rotation", "Lcl Rotation", "", "A", Vector3.zero);
            Property("Lcl Scaling", "Lcl Scaling", "", "A", Vector3.one);
            Property("Visibility", "Visibility", "", "A", 1);
            Property("Visibility Inheritance", "Visibility Inheritance", "", "", 1);
        }
    }
}
