﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using OpCode = dnlib.DotNet.Emit.OpCode;
using ReflOpCode = System.Reflection.Emit.OpCode;
using OpCodes = dnlib.DotNet.Emit.OpCodes;
using ReflOpCodes = System.Reflection.Emit.OpCodes;
using Code = dnlib.DotNet.Emit.Code;

namespace denvlib.Services
{
    public static class NETUtils
    {
        public static ModuleWriterListener listener = new ModuleWriterListener();

        public static MethodDef GetMethodByName(TypeDef type, string name)
        {
            return type.Methods.Single(method => method.Name == name);
        }

        public static TypeDef GetTypeByName(ModuleDef module, string name)
        {
            return module.Types.Single(type => type.Name == name);
        }

        public static TypeDef ImportType(Type type)
        {
            var module = ModuleDefMD.Load(type.Module);
            var td = module.ResolveTypeDef(MDToken.ToRID(type.MetadataToken));
            module.Types.Remove(td);
            return td;
        }

        public static ReflOpCode ToReflectionOp(this OpCode op)
        {
            switch (op.Code)
            {
                case Code.Add: return ReflOpCodes.Add;
                case Code.Mul: return ReflOpCodes.Mul;
                case Code.Sub: return ReflOpCodes.Sub;
                case Code.And: return ReflOpCodes.And;
                case Code.Xor: return ReflOpCodes.Xor;
                case Code.Or: return ReflOpCodes.Or;
                case Code.Ldc_I4: return ReflOpCodes.Ldc_I4;
                case Code.Ldarg_0: return ReflOpCodes.Ldarg_0;
                case Code.Ret: return ReflOpCodes.Ret;
                default: throw new NotImplementedException();
            }
        }

        public class ModuleWriterListener : IModuleWriterListener
        {
            public event EventHandler<ModuleWriterListenerEventArgs> OnWriterEvent;

            void IModuleWriterListener.OnWriterEvent(ModuleWriterBase writer, ModuleWriterEvent evt)
            {
                if (OnWriterEvent != null)
                    OnWriterEvent(writer, new ModuleWriterListenerEventArgs(evt));
            }

            public class ModuleWriterListenerEventArgs : EventArgs
            {
                public ModuleWriterListenerEventArgs(ModuleWriterEvent evt)
                {
                    this.WriterEvent = evt;
                }

                public ModuleWriterEvent WriterEvent { get; private set; }
            }
        }
    }
}
