using CleanArcClientFeature.Domain.ValueObjects;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System.Data.Common;


namespace CleanArcClientFeature.Infrastructure.Types;

public class CnpjTipo : IUserType
{
    public SqlType[] SqlTypes => new SqlType[] { new StringSqlType(14) };

    public Type ReturnedType => typeof(Cnpj);

    public new bool Equals(object x, object y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x == null || y == null) return false;
        return x.Equals(y);
    }

    public int GetHashCode(object x) => x?.GetHashCode() ?? 0;

    public object DeepCopy(object value)
    {
        if (value == null) return null;
        var cnpj = (Cnpj)value;
        return new Cnpj(cnpj.Value);
    }

    public bool IsMutable => false;

    public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
    {
        var value = rs[names[0]];
        if (value == DBNull.Value || value == null)
            return null;

        var strValue = (string)value;
        
        return Cnpj.Criar(strValue);
    }

    public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
    {
        if (value == null)
        {
            cmd.Parameters[index].Value = DBNull.Value;
        }
        else
        {
            var cnpj = (Cnpj)value;
            cmd.Parameters[index].Value = cnpj.Value;
        }
    }

    public object Replace(object original, object target, object owner) => original;
    public object Assemble(object cached, object owner) => cached;
    public object Disassemble(object value) => value;
}