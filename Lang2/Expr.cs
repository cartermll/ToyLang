using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Lang2
{
    internal abstract class Expr
    {
        public Expr right;
        public Expr left;
        public Token op;
    }

    internal class BinaryExpr : Expr
    {
        public BinaryExpr(Expr left, Expr right, Token op)
        {
            this.left = left;
            this.right = right;
            this.op = op;
        }
    }

    internal class UnaryExpr : Expr
    {
        public UnaryExpr (Expr left, Token op)
        {
            this.left = left;
            this.op = op;
        }
    }

    internal class LiteralExpr : Expr
    {
        public LiteralExpr(Token literal)
        {
            this.op = literal;
        }
    }
}
