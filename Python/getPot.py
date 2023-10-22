# import argparse
# from peaceful_pie.unity_comms import UnityComms

# def run(args: argparse.Namespace) -> None:
#     unity_comms = UnityComms(port=args.port)
#     unity_comms.GetPot(pot_amount=args.pot)

# if __name__ == "__main__":
#     parser = argparse.ArgumentParser()
#     parser.add_argument("--pot", type=str)
#     parser.add_argument("--port", type=int, default=9000)
#     args = parser.parse_args()
#     run(args)

import argparse
import subprocess
from peaceful_pie.unity_comms import UnityComms

def run(args):
    unity_comms = UnityComms(port=9000)
    unity_comms.GetPot(pot_amount=args.pot)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Get the current pot amount from the Unity game.")
    parser.add_argument("--pot", type=str, help="The current pot amount.")
    args = parser.parse_args()
    run(args)