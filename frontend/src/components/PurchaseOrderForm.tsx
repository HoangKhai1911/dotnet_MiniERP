import { useState } from "react";
import api from "../api";

export default function PurchaseOrderForm() {
  const [supplierId, setSupplierId] = useState<number>(1);
  const [productId, setProductId] = useState<number>(0);
  const [quantity, setQuantity] = useState<number>(1);
  const [unitPrice, setUnitPrice] = useState<number>(0);
  const [orderId, setOrderId] = useState<number | null>(null);
  const [message, setMessage] = useState("");

  const handleCreate = async () => {
    try {
      const res = await api.post("/purchaseorders", {
        supplierId,
        items: [{ productId, quantity, unitPrice }]
      });
      setOrderId(res.data.id);
      setMessage("Đơn nhập đã tạo thành công");
    } catch {
      setMessage("Tạo đơn nhập thất bại");
    }
  };

  const handleReceive = async () => {
    if (!orderId) return;
    try {
      await api.post(`/purchaseorders/${orderId}/receive`);
      setMessage("Đơn nhập đã nhận hàng, tồn kho tăng");
    } catch {
      setMessage("Nhận hàng thất bại");
    }
  };

  return (
    <div>
      <h3>Tạo đơn nhập</h3>
      <input type="number" value={supplierId} onChange={e => setSupplierId(Number(e.target.value))} placeholder="SupplierId" />
      <input type="number" value={productId} onChange={e => setProductId(Number(e.target.value))} placeholder="ProductId" />
      <input type="number" value={quantity} onChange={e => setQuantity(Number(e.target.value))} placeholder="Số lượng" />
      <input type="number" value={unitPrice} onChange={e => setUnitPrice(Number(e.target.value))} placeholder="Đơn giá" />
      <button onClick={handleCreate}>Tạo đơn nhập</button>
      {orderId && <button onClick={handleReceive}>Nhận hàng</button>}
      <p>{message}</p>
    </div>
  );
}
