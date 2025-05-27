import streamlit as st
from pythonosc.udp_client import SimpleUDPClient

st.set_page_config(page_title="Set OSC Client IPs", layout="centered")

st.title("OSC Client IP Setup")
st.write("Set up bidirectional OSC clients for:")
st.markdown("- **Interactive Table** ↔ **VR Headset**")

with st.form("ip_form"):
    table_ip = st.text_input("👋🏻 Interactive Table IP 👋🏻", value="192.168.1.10")
    headset_ip = st.text_input("🥽 VR Headset IP 🥽", value="192.168.1.11")
    port = st.number_input("OSC Port (same on both)", value=19639, min_value=1, max_value=65535)
    submitted = st.form_submit_button("Send /setip Commands")

if submitted:
    try:
        # Send to VR Headset: Give it the Table's IP
        vr_client = SimpleUDPClient(headset_ip, port)
        vr_client.send_message("/setip", [table_ip])
        st.success(f"Sent Table IP ({table_ip}) to VR Headset at {headset_ip}:{port}")

        # Send to Table: Give it the Headset's IP
        table_client = SimpleUDPClient(table_ip, port)
        table_client.send_message("/setip", [headset_ip])
        st.success(f"Sent VR Headset IP ({headset_ip}) to Table at {table_ip}:{port}")

    except Exception as e:
        st.error(f"Failed to send OSC messages: {e}")
