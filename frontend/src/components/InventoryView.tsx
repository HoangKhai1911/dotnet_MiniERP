import { useState } from "react";
import api from "../api";

interface Inventory {
  productId: number;
  warehouseId: number;
  warehouseName: string;
  quantity: number;
}

export default function InventoryView() {
  const [productId, setProductId] = useState<number>(0);
  const [inventory, setInventory] = useState<Inventory[]>([]);

  const handleSearch = async () => {
    const res = await api.get(`/inventory/${productId}`);
    setInventory(res.data);
  };

  return (
    <div>
      <h3>Tồn kho theo sản phẩm</h3>
      <input
        type="number"
        value={productId}
        onChange={e => setProductId(Number(e.target.value))}
        placeholder="Nhập ProductId"
      />
      <button onClick={handleSearch}>Xem tồn kho</button>

      <ul>
        {inventory.map(i => (
          <li key={i.warehouseId}>
            Kho: {i.warehouseName} — Số lượng: {i.quantity}
          </li>
        ))}
      </ul>
    </div>
  );
}
