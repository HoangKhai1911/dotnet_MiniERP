import React, { useState } from 'react';
import { Layout, Menu, Button, Typography, Dropdown } from 'antd';
import { 
  DashboardOutlined, 
  AppstoreOutlined, 
  ShoppingCartOutlined, 
  UserOutlined,
  LogoutOutlined,
  DropboxOutlined
} from '@ant-design/icons';
import { useNavigate, useLocation, Outlet } from 'react-router-dom';

const { Header, Sider, Content } = Layout;
const { Title } = Typography;

const MainLayout: React.FC = () => {
  const [collapsed, setCollapsed] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();

  // Xử lý sự kiện đăng xuất
  const handleLogout = () => {
    localStorage.removeItem('token'); // Xóa Token khỏi trình duyệt
    navigate('/login'); // Điều hướng về trang đăng nhập
  };

  // Cấu hình thanh Menu bên trái
  const menuItems = [
    { key: '/', icon: <DashboardOutlined />, label: 'Trang chủ' },
    { key: '/inventory', icon: <DropboxOutlined />, label: 'Quản lý Tồn kho' },
    { key: '/products', icon: <AppstoreOutlined />, label: 'Quản lý Sản phẩm' },
    { key: '/customers', icon: <UserOutlined />, label: 'Khách hàng' },
    { key: '/orders', icon: <ShoppingCartOutlined />, label: 'Đơn hàng' },
  ];

  // Cấu hình menu thả xuống của User ở góc trên bên phải
  const userMenu = {
    items: [
      { 
        key: 'logout', 
        icon: <LogoutOutlined />, 
        label: 'Đăng xuất', 
        onClick: handleLogout,
        danger: true // Hiện màu đỏ cho nút đăng xuất
      },
    ],
  };

  return (
    <Layout style={{ minHeight: '100vh' }}>
      {/* Cột Menu bên trái (Sider) */}
      <Sider 
        collapsible 
        collapsed={collapsed} 
        onCollapse={(value) => setCollapsed(value)}
        theme="dark"
      >
        <div style={{ 
          height: 32, 
          margin: 16, 
          background: 'rgba(255, 255, 255, 0.2)', 
          borderRadius: 6,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          color: 'white',
          fontWeight: 'bold',
          overflow: 'hidden',
          whiteSpace: 'nowrap'
        }}>
          {collapsed ? 'ERP' : 'MINI ERP SYSTEM'}
        </div>
        
        <Menu 
          theme="dark" 
          mode="inline" 
          selectedKeys={[location.pathname]} // Tự động highlight menu dựa trên URL hiện tại
          items={menuItems}
          onClick={({ key }) => navigate(key)} // Điều hướng khi bấm vào menu
        />
      </Sider>

      {/* Khu vực nội dung chính bên phải */}
      <Layout>
        {/* Thanh Header (Chứa tiêu đề và nút User) */}
        <Header style={{ 
          padding: '0 24px', 
          background: '#fff', 
          display: 'flex', 
          justifyContent: 'space-between', 
          alignItems: 'center',
          boxShadow: '0 1px 4px rgba(0,21,41,.08)',
          zIndex: 1
        }}>
          <Title level={4} style={{ margin: 0 }}>Hệ Thống Quản Trị ERP</Title>
          
          <Dropdown menu={userMenu} placement="bottomRight" arrow>
            <Button type="text" size="large" icon={<UserOutlined />}>
              Quản trị viên
            </Button>
          </Dropdown>
        </Header>

        {/* Khung chứa các trang con (Dashboard, Products,...) */}
        <Content style={{ margin: '24px 16px', display: 'flex', flexDirection: 'column' }}>
          <div style={{ 
            padding: 24, 
            minHeight: 360, 
            background: '#fff', 
            borderRadius: 8,
            flex: 1 // Giúp khung nền trắng luôn kéo dài hết chiều cao còn lại
          }}>
            {/* <Outlet /> là nơi các Component như Dashboard, Products, Inventory... 
               được React Router tự động "bơm" vào dựa trên URL.
            */}
            <Outlet />
          </div>
        </Content>
      </Layout>
    </Layout>
  );
};

export default MainLayout;