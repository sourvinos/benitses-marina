using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.Internal;

namespace API.Infrastructure.Extensions {

    public static class EFQueries {

        private static Expression<Func<TOuter, TInner, TResult>> CastSMLambda<TOuter, TInner, TResult>(LambdaExpression ex, TOuter _1, TInner _2, TResult _3) => (Expression<Func<TOuter, TInner, TResult>>)ex;

        public static IQueryable<TResult> LeftOuterJoin<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IQueryable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeyExpr, Expression<Func<TInner, TKey>> innerKeyExpr, Expression<Func<TOuter, TInner, TResult>> resExpr) {
            var gjResTemplate = new { outer = default(TOuter), innerj = default(IEnumerable<TInner>) };
            var oijParm = Expression.Parameter(gjResTemplate.GetType(), "oij");
            var iParm = Expression.Parameter(typeof(TInner), "inner");
            var oijOuter = Expression.PropertyOrField(oijParm, "outer");
            var selectResExpr = CastSMLambda(Expression.Lambda(resExpr.Apply(oijOuter, iParm), oijParm, iParm), gjResTemplate, default(TInner), default(TResult));
            return outer.GroupJoin(inner, outerKeyExpr, innerKeyExpr, (outer, innerj) => new { outer, innerj })
                        .SelectMany(r => r.innerj.DefaultIfEmpty(), selectResExpr);
        }

        public static Expression Apply(this LambdaExpression e, params Expression[] args) {
            var b = e.Body;
            foreach (var pa in e.Parameters.Zip(args, (p, a) => (p, a)))
                b = b.Replace(pa.p, pa.a);
            return b.PropagateNull();
        }

        public static T Replace<T>(this T orig, Expression from, Expression to) where T : Expression => (T)new ReplaceVisitor(from, to).Visit(orig);

        public class ReplaceVisitor : ExpressionVisitor {
            readonly Expression from;
            readonly Expression to;
            public ReplaceVisitor(Expression from, Expression to) {
                this.from = from;
                this.to = to;
            }
            public override Expression Visit(Expression node) => node == from ? to : base.Visit(node);
        }

        public static T PropagateNull<T>(this T orig) where T : Expression => (T)new NullVisitor().Visit(orig);

        public class NullVisitor : ExpressionVisitor {
            public override Expression Visit(Expression node) {
                if (node is MemberExpression nme && nme.Expression is ConstantExpression nce && nce.Value == null)
                    return Expression.Constant(null, nce.Type.GetMember(nme.Member.Name).Single().GetMemberType());
                else
                    return base.Visit(node);
            }
        }

    }

}