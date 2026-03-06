import { Outlet } from "react-router-dom";
import { useEffect, useState } from "react";
import styled from "styled-components";
import Navbar from "./Navbar";
import Sidebar from "./Sidebar";

export default function AppLayout() {
  const [collapsed, setCollapsed] = useState(false);   
  const [mobileOpen, setMobileOpen] = useState(false);  

  useEffect(() => {
    const onResize = () => {
      if (window.innerWidth >= 900) setMobileOpen(false);
    };
    window.addEventListener("resize", onResize);
    return () => window.removeEventListener("resize", onResize);
  }, []);

  return (
    <Shell>
      <Navbar
        onHamburger={() => setMobileOpen(true)}
        onToggleSidebar={() => setCollapsed((v) => !v)}
      />

      <Body>
        <Overlay
          $open={mobileOpen}
          onClick={() => setMobileOpen(false)}
          aria-hidden={!mobileOpen}
        />

        <Sidebar
          collapsed={collapsed}
          mobileOpen={mobileOpen}
          onCloseMobile={() => setMobileOpen(false)}
          onToggleDesktop={() => setCollapsed((v) => !v)}
        />

        <Content $collapsed={collapsed}>
          <Outlet />
        </Content>
      </Body>
    </Shell>
  );
}

const Shell = styled.div`
  min-height: 100vh;
  background: #f6f7fb;
`;

const Body = styled.div`
  display: flex;
`;

const Overlay = styled.div<{ $open: boolean }>`
  position: fixed;
  inset: 56px 0 0 0;
  background: rgba(15, 23, 42, 0.45);
  opacity: ${({ $open }) => ($open ? 1 : 0)};
  pointer-events: ${({ $open }) => ($open ? "auto" : "none")};
  transition: opacity 0.2s ease;
  z-index: 19;

  @media (min-width: 900px) {
    display: none;
  }
`;

const Content = styled.main<{ $collapsed: boolean }>`
  flex: 1;
  padding: 16px;

  /* desktop: deixa espaço pro sidebar */
  margin-left: ${({ $collapsed }) => ($collapsed ? "64px" : "240px")};
  transition: margin-left 0.2s ease;

  @media (max-width: 899px) {
    margin-left: 0;
    padding: 12px;
  }
`;