import streamlit as st
from pythonosc.udp_client import SimpleUDPClient
from datetime import datetime

st.set_page_config(page_title="OSC Sender", layout="centered")

st.title("📡 Wi-Fi OSC Message Sender")

# Initialize session state for logs
if "log" not in st.session_state:
    st.session_state.log = []

# Configuration
ip = st.text_input("Receiver IP Address", value="192.168.1.100")
port = st.number_input("Receiver Port", min_value=1, max_value=65535, value=8000)
osc_address = st.text_input("OSC Address", value="/test")

st.markdown("### 📦 Message Arguments")
args = []
num_args = st.number_input("Number of Arguments", min_value=0, max_value=10, value=1)

for i in range(num_args):
    col1, col2 = st.columns(2)
    with col1:
        arg_type = st.selectbox(f"Type of argument {i+1}", ["int", "float", "string"], key=f"type_{i}")
    with col2:
        arg_value = st.text_input(f"Value for argument {i+1}", key=f"value_{i}")
    
    # Convert and append the argument
    try:
        if arg_type == "int":
            args.append(int(arg_value))
        elif arg_type == "float":
            args.append(float(arg_value))
        else:
            args.append(arg_value)
    except ValueError:
        st.error(f"Invalid value for argument {i+1} ({arg_type}): {arg_value}")

# Send OSC Message
if st.button("Send OSC Message"):
    try:
        client = SimpleUDPClient(ip, port)
        client.send_message(osc_address, args)

        # Log the message
        timestamp = datetime.now().strftime("%H:%M:%S")
        log_entry = f"[{timestamp}] Sent to {ip}:{port} | Address: {osc_address} | Args: {args}"
        st.session_state.log.append(log_entry)
        
        st.success("Message sent successfully!")

    except Exception as e:
        error_msg = f"Error sending OSC message: {e}"
        st.session_state.log.append(error_msg)
        st.error(error_msg)

# Display Logs
st.markdown("### 📝 Message Log")
with st.expander("View Message Log", expanded=True):
    if st.session_state.log:
        st.markdown("<div style='max-height: 200px; overflow-y: auto; font-family: monospace;'>"
                    + "<br>".join(st.session_state.log) +
                    "</div>", unsafe_allow_html=True)
    else:
        st.info("No messages sent yet.")
