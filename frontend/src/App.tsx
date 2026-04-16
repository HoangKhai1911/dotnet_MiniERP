import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';

// Import Layout và các Trang
import MainLayout from './components/MainLayout';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import Inventory from './pages/Inventory';
import Products from './pages/Products';
import Customers from './pages/Customers';
import Orders from './pages/Orders';

// Component Bảo vệ (Kiểm tra xem đã có Token chưa)
const ProtectedRoute = ({ children }: { children: JSX.Element }) => {
  const token = localStorage.getItem('token');
  if (!token) {
    return <Navigate to="/login" replace />;
  }
  return children;
};

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        {/* Tuyến đường tự do (Không cần đăng nhập) */}
        <Route path="/login" element={<Login />} />

        {/* Các tuyến đường được bảo vệ (Nằm bên trong MainLayout) */}
        <Route 
          path="/" 
          element={
            <ProtectedRoute>
              <MainLayout />
            </ProtectedRoute>
          }
        >
          <Route index element={<Dashboard />} />
          <Route path="inventory" element={<Inventory />} />
          <Route path="products" element={<Products />} />
          <Route path="customers" element={<Customers />} />
          {/* Đã bổ sung Route cho trang Đơn hàng */}
          <Route path="orders" element={<Orders />} />
        </Route>

        {/* Xử lý gõ sai URL (404) -> Trả về trang chủ */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </Router>
  );
};

export default App;