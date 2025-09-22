 UPDATE stk
            SET   stk.CurStock = stk.OpStock + stk.InStock -  stk.OutStock 
			FROM tblProdStock_Dtl stk