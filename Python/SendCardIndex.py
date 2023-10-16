import argparse
from peaceful_pie.unity_comms import UnityComms

def run(args: argparse.Namespace) -> None:
    unity_comms = UnityComms(port=args.port)
    unity_comms.SendCommand(cardIdx=args.comm)

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--comm", type=str, required=True)
    parser.add_argument("--port", type=int, default=9000)
    args = parser.parse_args()
    run(args)